// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : ApiControllerBase.cs
// Created          : 2015-07-27  12:43 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  7:01 PM
// ***********************************************************************
// <copyright file="ApiControllerBase.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Tracing;
using System.Web.Security;
using Moe.AspNet.Utility;
using Moe.Lib;
using Orleans;
using Orleans.Runtime.Host;

namespace Yuyi.Jinyinmao.Api.Controllers
{
    /// <summary>
    ///     Class ApiControllerBase.
    /// </summary>
    public abstract class ApiControllerBase : ApiController
    {
        /// <summary>
        ///     The current user
        /// </summary>
        private CurrentUser currentUser;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiControllerBase" /> class.
        /// </summary>
        protected ApiControllerBase()
        {
            if (AzureClient.IsInitialized || GrainClient.IsInitialized)
            {
                return;
            }

#if DEBUG
            GrainClient.Initialize(HttpContext.Current.Server.MapPath(@"~/DebugConfiguration.xml"));
#elif CLOUD
            AzureClient.Initialize(HttpContext.Current.Server.MapPath(@"~/AzureConfiguration.xml"));
#else
            GrainClient.Initialize(HttpContext.Current.Server.MapPath(@"~/ReleaseConfiguration.xml"));
#endif
        }

        /// <summary>
        ///     Gets the current user.
        /// </summary>
        /// <value>The current user.</value>
        protected CurrentUser CurrentUser => this.currentUser ?? (this.currentUser = this.GetCurrentUser());

        /// <summary>
        ///     Gets the trace writer.
        /// </summary>
        /// <value>The trace writer.</value>
        protected ITraceWriter TraceWriter
        {
            get { return this.Configuration.Services.GetTraceWriter(); }
        }

        /// <summary>
        ///     Builds the arguments.
        /// </summary>
        /// <returns>System.String.</returns>
        protected Dictionary<string, object> BuildArgs(Dictionary<string, object> argsToAdd = null)
        {
            List<KeyValuePair<string, string>> args = this.Request.GetQueryNameValuePairs().Where(kv => !kv.Key.Contains("password", StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<KeyValuePair<string, IEnumerable<string>>> headers = this.Request.Headers.Where(h => h.Key.Contains("X-JYM", StringComparison.InvariantCultureIgnoreCase)).ToList();

            Dictionary<string, object> argsDictionary = new Dictionary<string, object>
            {
                { "Ip", HttpUtils.GetUserHostAddress(this.Request) },
                { "UserAgent", HttpUtils.GetUserAgent(this.Request) }
            };

            foreach (KeyValuePair<string, string> arg in args)
            {
                if (argsDictionary.ContainsKey(arg.Key))
                {
                    argsDictionary[arg.Key] = arg.Value;
                }
                else
                {
                    argsDictionary.Add(arg.Key, arg.Value);
                }
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
            {
                if (argsDictionary.ContainsKey(header.Key))
                {
                    argsDictionary[header.Key] = header.Value.Join(";");
                }
                else
                {
                    argsDictionary.Add(header.Key, header.Value.Join(";"));
                }
            }

            if (argsToAdd != null)
            {
                foreach (KeyValuePair<string, object> arg in argsToAdd)
                {
                    if (argsDictionary.ContainsKey(arg.Key))
                    {
                        argsDictionary[arg.Key] = arg.Value;
                    }
                    else
                    {
                        argsDictionary.Add(arg.Key, arg.Value);
                    }
                }
            }

            return argsDictionary;
        }

        /// <summary>
        ///     Gets the current user.
        /// </summary>
        /// <returns>CurrentUser.</returns>
        private CurrentUser GetCurrentUser()
        {
            IPrincipal principal = null;
            IEnumerable<string> header;
            if (this.Request.Headers.TryGetValues("X-JYM-AUTH", out header))
            {
                string tokenValue = header.FirstOrDefault();
                if (tokenValue.IsNotNullOrEmpty())
                {
                    try
                    {
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(tokenValue);
                        if (ticket != null) principal = new GenericPrincipal(new GenericIdentity(ticket.Name), null);
                    }
                    catch (Exception)
                    {
                        // ignore
                    }
                }
            }

            if (principal == null)
            {
                principal = this.User;
            }

            if (principal?.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }

            string token = principal.Identity.Name;
            string[] tokens = token.Split(',');

            if (string.IsNullOrWhiteSpace(token) || tokens.Length != 3)
            {
                return null;
            }

            DateTime expiryTime = DateTime.MinValue;
            long expiry;
            if (long.TryParse(tokens[2], out expiry))
            {
                expiryTime = DateTime.FromBinary(expiry);
            }

            return new CurrentUser
            {
                Id = Guid.Parse(tokens[0]),
                Cellphone = tokens[1],
                ExpiryTime = expiryTime
            };
        }

        /// <summary>
        ///     Creates an <see cref="T:System.Web.Http.IHttpActionResult" /> (200 OK).
        /// </summary>
        /// <returns>An <see cref="T:System.Web.Http.IHttpActionResult" /> (200 OK).</returns>
        protected new IHttpActionResult Ok()
        {
            return base.Ok(new object());
        }
    }

    /// <summary>
    ///     Class CurrentUser.
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the expiry time.
        /// </summary>
        /// <value>The expiry time.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public DateTime ExpiryTime { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }
    }
}