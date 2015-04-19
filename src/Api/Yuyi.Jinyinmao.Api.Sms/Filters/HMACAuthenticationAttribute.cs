// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  1:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  2:32 PM
// ***********************************************************************
// <copyright file="HMACAuthenticationAttribute.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
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
using Yuyi.Jinyinmao.Api.Sms.Models;

namespace Yuyi.Jinyinmao.Api.Sms.Filters
{
    /// <summary>
    ///     Class HMACAuthenticationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HMACAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private static readonly string authenticationScheme = "jas";

        private static readonly TableQuery<App> query = new TableQuery<App>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
            QueryComparisons.Equal, "cn.com.jinyinmao.api.sms.config.appkeys"));

        private static readonly long requestMaxAgeInSeconds = 300;
        private static readonly CloudStorageAccount storageAccount;
        private static Dictionary<Guid, App> allowedApps = new Dictionary<Guid, App>();
        private static DateTime configLoadTime;

        /// <summary>
        ///     Initializes static members of the <see cref="HMACAuthenticationAttribute" /> class.
        /// </summary>
        static HMACAuthenticationAttribute()
        {
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            LoadAppKeysConfig();
        }

        #region IAuthenticationFilter Members

        /// <summary>
        ///     Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.
        /// </summary>
        /// <returns>
        ///     true if more than one instance is allowed to be specified; otherwise, false. The default is false.
        /// </returns>
        public bool AllowMultiple
        {
            get { return false; }
        }

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
            LoadAppKeysConfig();

            HttpRequestMessage req = context.Request;

            if (req.Headers.Authorization != null && authenticationScheme.Equals(req.Headers.Authorization.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                string rawAuthzHeader = req.Headers.Authorization.Parameter;

                string[] autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);

                if (autherizationHeaderArray != null)
                {
                    string APPId = autherizationHeaderArray[0];
                    string incomingBase64Signature = autherizationHeaderArray[1];
                    string nonce = autherizationHeaderArray[2];
                    string requestTimeStamp = autherizationHeaderArray[3];

                    Task<bool> isValid = isValidRequest(req, APPId, incomingBase64Signature, nonce, requestTimeStamp);

                    if (isValid.Result)
                    {
                        GenericPrincipal currentPrincipal = new GenericPrincipal(new GenericIdentity(APPId), null);
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

        private static void LoadAppKeysConfig()
        {
            if (DateTime.UtcNow - configLoadTime > TimeSpan.FromMinutes(5))
            {
                configLoadTime = DateTime.UtcNow;
                CloudTableClient client = storageAccount.CreateCloudTableClient();

                IEnumerable<App> apps = client.GetTableReference("ApiSms").ExecuteQuery(query);
                Dictionary<Guid, App> tempAllowedApps = apps.Where(a => a.Expiry > DateTime.Now).ToDictionary(app => app.AppId);
                allowedApps = tempAllowedApps;
            }
        }

        private string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {
            string[] credArray = rawAuthzHeader.Split(':');

            return credArray.Length == 4 ? credArray : null;
        }

        private bool isReplayRequest(string nonce, string requestTimeStamp)
        {
            if (MemoryCache.Default.Contains(nonce))
            {
                return true;
            }

            long serverTotalSeconds = DateTime.UtcNow.UnixTimeStamp();
            long requestTotalSeconds = Convert.ToInt64(requestTimeStamp);

            if ((serverTotalSeconds - requestTotalSeconds) > requestMaxAgeInSeconds)
            {
                return true;
            }

            MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(requestMaxAgeInSeconds));

            return false;
        }

        private async Task<bool> isValidRequest(HttpRequestMessage req, string APPId, string incomingBase64Signature, string nonce, string requestTimeStamp)
        {
            string requestContentBase64String = "";
            string requestUri = HttpUtility.UrlEncode(req.RequestUri.AbsoluteUri.ToLower());
            string requestHttpMethod = req.Method.Method;

            Guid appId;
            if (!Guid.TryParse(APPId, out appId))
            {
                return false;
            }

            if (!allowedApps.ContainsKey(appId))
            {
                return false;
            }

            App app = allowedApps[appId];

            if (isReplayRequest(nonce, requestTimeStamp))
            {
                return false;
            }

            byte[] hash = await ComputeHash(req.Content);

            if (hash != null)
            {
                requestContentBase64String = Convert.ToBase64String(hash);
            }

            string data = String.Format("{0}{1}{2}{3}{4}{5}", APPId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            byte[] secretKeyBytes = Convert.FromBase64String(app.AppKey);

            byte[] signature = Encoding.UTF8.GetBytes(data);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);

                return (incomingBase64Signature.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal));
            }
        }

        #region Nested type: ResultWithChallenge

        /// <summary>
        ///     ResultWithChallenge.
        /// </summary>
        public class ResultWithChallenge : IHttpActionResult
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
                HttpResponseMessage response = await next.ExecuteAsync(cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(authenticationScheme));
                }

                return response;
            }

            #endregion IHttpActionResult Members
        }

        #endregion Nested type: ResultWithChallenge
    }
}
