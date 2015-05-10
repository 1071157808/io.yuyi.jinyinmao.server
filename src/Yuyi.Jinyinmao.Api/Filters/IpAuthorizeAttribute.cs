// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  9:13 AM
// ***********************************************************************
// <copyright file="IpAuthorizeAttribute.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
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
    public class IpAuthorizeAttribute : OrderedAuthorizationFilterAttribute
    {
        /// <summary>
        ///     Calls when a process requests authorization.
        /// </summary>
        /// <param name="actionContext">The action context, which encapsulates information for using <see cref="T:System.Web.Http.Filters.AuthorizationFilterAttribute" />.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!this.IpIsAuthorized(actionContext.Request))
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
                throw new ArgumentNullException("actionContext", "actionContext can not be null");
            }

            actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "");
        }

        /// <summary>
        ///     Determines whether the client ip is authorized.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///     bool
        /// </returns>
        private bool IpIsAuthorized(HttpRequestMessage request)
        {
            string ip = HttpUtils.GetUserHostAddress(request);
            return !String.IsNullOrEmpty(ip) && (ip.StartsWith("172.26") || ip.StartsWith("172.25") || ip.StartsWith("10.1") || ip == "::1");
        }
    }
}