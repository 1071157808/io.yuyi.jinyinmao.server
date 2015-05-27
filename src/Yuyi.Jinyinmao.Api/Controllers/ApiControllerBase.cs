// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-26  9:55 PM
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
using Moe.AspNet.Utility;
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
            GrainClient.Initialize(HttpContext.Current.Server.MapPath(@"~/LocalDevConfiguration.xml"));
#elif CLOUD
            AzureClient.Initialize(HttpContext.Current.Server.MapPath(@"~/AzureConfiguration.xml"));
#else
            GrainClient.Initialize(HttpContext.Current.Server.MapPath(@"~/LocalConfiguration.xml"));
#endif
        }

        /// <summary>
        ///     Gets the trace.
        /// </summary>
        /// <value>The trace.</value>
        public ITraceWriter Trace
        {
            get { return this.Configuration.Services.GetTraceWriter(); }
        }

        /// <summary>
        ///     Gets the current user.
        /// </summary>
        /// <value>The current user.</value>
        protected CurrentUser CurrentUser
        {
            get { return this.currentUser ?? (this.currentUser = this.GetCurrentUser()); }
        }

        /// <summary>
        ///     Builds the arguments.
        /// </summary>
        /// <returns>System.String.</returns>
        protected Dictionary<string, object> BuildArgs(Dictionary<string, object> argsToAdd = null)
        {
            List<KeyValuePair<string, string>> args = this.Request.GetQueryNameValuePairs().ToList();
            args.Add(new KeyValuePair<string, string>("Ip", HttpUtils.GetUserHostAddress(this.Request)));
            args.Add(new KeyValuePair<string, string>("UserAgent", HttpUtils.GetUserAgent(this.Request)));

            Dictionary<string, object> argsDictionary = new Dictionary<string, object>();

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
        [SuppressMessage("ReSharper", "MergeSequentialChecks")]
        protected CurrentUser GetCurrentUser()
        {
            IPrincipal principal = this.User;

            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return null;
            }

            string token = principal.Identity.Name;
            string[] tokens = token.Split(',');

            if (String.IsNullOrWhiteSpace(token) || tokens.Length != 3)
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
        public DateTime ExpiryTime { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }
    }
}