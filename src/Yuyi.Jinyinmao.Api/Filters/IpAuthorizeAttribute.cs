// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IpAuthorizeAttribute.cs
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-11  10:22 AM
// ***********************************************************************
// <copyright file="IpAuthorizeAttribute.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using Moe.AspNet.Filters;
using Moe.AspNet.Utility;

namespace Yuyi.Jinyinmao.Api.Filters
{
    /// <summary>
    ///     IpAuthorizeAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class IpAuthorizeAttribute : OrderedAuthorizationFilterAttribute
    {
        /// <summary>
        ///     Gets or sets a value indicating whether [only local host].
        /// </summary>
        /// <value><c>true</c> if [only local host]; otherwise, <c>false</c>.</value>
        public bool OnlyLocalHost { get; set; }

        [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Local")]
        private string[] AdminIps { get; } = { "10.1.10.100", "10.1.10.13", "10.1.10.31", "10.1.10.64", "10.1.5.42" };

        [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Local")]
        private string[] AllowedIps { get; } = { "101.95.30.142", "211.152.53.50" };

        /// <summary>
        ///     Calls when a process requests authorization.
        /// </summary>
        /// <param name="actionContext">The action context, which encapsulates information for using <see cref="T:System.Web.Http.Filters.AuthorizationFilterAttribute" />.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!this.IpIsAuthorized(actionContext))
            {
                this.HandleUnauthorizedRequest(actionContext);
            }

            base.OnAuthorization(actionContext);
        }

        /// <summary>
        ///     Processes requests that fail authorization. This default implementation creates a new response with the
        ///     Unauthorized status code. Override this method to provide your own handling for unauthorized requests.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        private void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext), @"actionContext can not be null");
            }

            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "");
        }

        private bool IpIsAuthorized(HttpActionContext context)
        {
            if (CookieAuthorizeAttribute.IsAdmin(context.RequestContext.Principal))
            {
                return true;
            }

            HttpRequestMessage request = context.Request;
            string ip = HttpUtils.GetUserHostAddress(request);

            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }

            if (this.OnlyLocalHost)
            {
                return request.IsLocal() || ip == "::1" || this.AdminIps.Contains(ip);
            }

            return this.AllowedIps.Contains(ip) || request.IsLocal() || ip == "::1" || this.AdminIps.Contains(ip);
        }
    }
}