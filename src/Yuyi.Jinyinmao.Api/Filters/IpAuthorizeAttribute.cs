// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:18 PM
// ***********************************************************************
// <copyright file="IpAuthorizeAttribute.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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

            var request = context.Request;
            string ip = HttpUtils.GetUserHostAddress(request);

            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }

            if (this.OnlyLocalHost)
            {
                return request.IsLocal() || ip == "::1";
            }

            return ip.StartsWith("172.26", StringComparison.Ordinal) || ip.StartsWith("172.25", StringComparison.Ordinal) || ip.StartsWith("10.1", StringComparison.Ordinal) || ip == "::1";
        }
    }
}