// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  6:23 PM
// ***********************************************************************
// <copyright file="HMACAuthenticationAttribute.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Moe.Lib;

namespace Yuyi.Jinyinmao.Api.Filters
{
    /// <summary>
    ///     App.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassCanBeSealed.Global")]
    public class App : TableEntity
    {
        /// <summary>
        ///     Gets or sets the application key.
        /// </summary>
        /// <value>The application key.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        [SuppressMessage("ReSharper", "MemberCanBeInternal")]
        public string ApiKey { get; set; }

        /// <summary>
        ///     Gets or sets the application identifier.
        /// </summary>
        /// <value>The application identifier.</value>
        [SuppressMessage("ReSharper", "MemberCanBeInternal")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public Guid AppId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string AppName { get; set; }

        /// <summary>
        ///     Gets or sets the expiry.
        /// </summary>
        /// <value>The expiry.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        [SuppressMessage("ReSharper", "MemberCanBeInternal")]
        public DateTime Expiry { get; set; }

        /// <summary>
        ///     Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }
    }

    /// <summary>
    ///     Class HMACAuthenticationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class HMACAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private static readonly string ApiKeyName = "ApiAdmin";
        private static readonly string AuthenticationScheme = "jas";
        private static readonly bool EnableAuth;

        private static readonly TableQuery<App> Query = new TableQuery<App>().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey",
            QueryComparisons.Equal, "jinyinmao"), TableOperators.And, "{0} eq true".FormatWith(ApiKeyName)));

        private static readonly long RequestMaxAgeInSeconds = 300;
        private static readonly CloudStorageAccount StorageAccount;
        private static Dictionary<Guid, App> allowedApps = new Dictionary<Guid, App>();
        private static DateTime configLoadTime = DateTime.MinValue;

        /// <summary>
        ///     Initializes static members of the <see cref="HMACAuthenticationAttribute" /> class.
        /// </summary>
        static HMACAuthenticationAttribute()
        {
            bool temp;
            string enableHMACAuthConfig = CloudConfigurationManager.GetSetting("EnableHMACAuth");
            EnableAuth = enableHMACAuthConfig.IsNotNullOrEmpty() && bool.TryParse(enableHMACAuthConfig, out temp) && temp;

            if (EnableAuth)
            {
                StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            }
        }

        #region IAuthenticationFilter Members

        /// <summary>
        ///     Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.
        /// </summary>
        /// <returns>
        ///     true if more than one instance is allowed to be specified; otherwise, false. The default is false.
        /// </returns>
        public bool AllowMultiple => false;

        /// <summary>
        ///     Authenticates the request.
        /// </summary>
        /// <returns>
        ///     A Task that will perform authentication.
        /// </returns>
        /// <param name="context">The authentication context.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (!EnableAuth)
            {
                GenericPrincipal currentPrincipal = new GenericPrincipal(new GenericIdentity("HMAC is disabled."), null);
                context.Principal = currentPrincipal;
                return Task.FromResult(0);
            }

            if (CookieAuthorizeAttribute.IsAdmin(context.Principal))
            {
                return Task.FromResult(0);
            }

            LoadAppKeysConfig();

            HttpRequestMessage req = context.Request;

            if (req.Headers.Authorization != null && AuthenticationScheme.Equals(req.Headers.Authorization.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                string rawAuthzHeader = req.Headers.Authorization.Parameter;

                string[] autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);

                if (autherizationHeaderArray != null)
                {
                    string appId = autherizationHeaderArray[0];
                    string incomingBase64Signature = autherizationHeaderArray[1];
                    string nonce = autherizationHeaderArray[2];
                    string requestTimeStamp = autherizationHeaderArray[3];

                    Task<bool> isValid = IsValidRequest(req, appId, incomingBase64Signature, nonce, requestTimeStamp);

                    if (isValid.Result)
                    {
                        GenericPrincipal currentPrincipal = new GenericPrincipal(new GenericIdentity(appId), null);
                        context.Principal = currentPrincipal;
                    }
                    else
                    {
                        context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                    }
                }
                else
                {
                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                }
            }
            else
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        ///     Challenges the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }

        #endregion IAuthenticationFilter Members

        private static async Task<byte[]> ComputeHash(HttpContent httpContent)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = null;
                byte[] content = await httpContent.ReadAsByteArrayAsync();
                if (content.Length != 0)
                {
                    hash = md5.ComputeHash(content);
                }
                return hash;
            }
        }

        private static string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {
            string[] credArray = rawAuthzHeader.Split(':');

            return credArray.Length == 4 ? credArray : null;
        }

        private static bool IsReplayRequest(string nonce, string requestTimeStamp)
        {
            if (MemoryCache.Default.Contains(nonce))
            {
                return true;
            }

            long serverTotalSeconds = DateTime.UtcNow.UnixTimeStamp();
            long requestTotalSeconds = Convert.ToInt64(requestTimeStamp);

            if ((serverTotalSeconds - requestTotalSeconds) > RequestMaxAgeInSeconds)
            {
                return true;
            }

            MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(RequestMaxAgeInSeconds));

            return false;
        }

        private static async Task<bool> IsValidRequest(HttpRequestMessage req, string appIdString, string incomingBase64Signature, string nonce, string requestTimeStamp)
        {
            string requestContentBase64String = "";
            string requestUri = HttpUtility.UrlEncode(req.RequestUri.AbsoluteUri.ToLower());
            string requestHttpMethod = req.Method.Method;

            Guid appId;
            if (!Guid.TryParse(appIdString, out appId))
            {
                return false;
            }

            if (!allowedApps.ContainsKey(appId))
            {
                return false;
            }

            App app = allowedApps[appId];

            if (IsReplayRequest(nonce, requestTimeStamp))
            {
                return false;
            }

            byte[] hash = await ComputeHash(req.Content);

            if (hash != null)
            {
                requestContentBase64String = Convert.ToBase64String(hash);
            }

            string data = $"{appIdString}{requestHttpMethod}{requestUri}{requestTimeStamp}{nonce}{requestContentBase64String}";

            byte[] secretKeyBytes = Convert.FromBase64String(app.ApiKey);

            byte[] signature = Encoding.UTF8.GetBytes(data);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);

                return (incomingBase64Signature.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal));
            }
        }

        private static void LoadAppKeysConfig()
        {
            if (DateTime.UtcNow - configLoadTime > TimeSpan.FromMinutes(5))
            {
                configLoadTime = DateTime.UtcNow;
                CloudTableClient client = StorageAccount.CreateCloudTableClient();

                IEnumerable<App> apps = client.GetTableReference("ApiKeys").ExecuteQuery(Query);
                Dictionary<Guid, App> tempAllowedApps = apps.Where(a => a.Expiry > DateTime.UtcNow.ToChinaStandardTime()).ToDictionary(app => app.AppId);
                allowedApps = tempAllowedApps;
            }
        }

        #region Nested type: ResultWithChallenge

        /// <summary>
        ///     ResultWithChallenge.
        /// </summary>
        private sealed class ResultWithChallenge : IHttpActionResult
        {
            private readonly string authenticationScheme = "jas";
            private readonly IHttpActionResult next;

            /// <summary>
            ///     Initializes a new instance of the <see cref="ResultWithChallenge" /> class.
            /// </summary>
            /// <param name="next">The next.</param>
            public ResultWithChallenge(IHttpActionResult next)
            {
                this.next = next;
            }

            #region IHttpActionResult Members

            /// <summary>
            ///     execute as an asynchronous operation.
            /// </summary>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
            public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = await this.next.ExecuteAsync(cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(this.authenticationScheme));
                }

                return response;
            }

            #endregion IHttpActionResult Members
        }

        #endregion Nested type: ResultWithChallenge
    }
}