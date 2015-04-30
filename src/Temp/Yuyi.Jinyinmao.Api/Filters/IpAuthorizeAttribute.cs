// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  10:02 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  10:03 AM
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
    public class IpAuthorizeAttribute : OrderedActionFilterAttribute
    {
        /// <summary>
        ///     Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!this.IpIsAuthorized(actionContext.Request))
            {
                this.HandleUnauthorizedRequest(actionContext);
            }
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
            return !String.IsNullOrEmpty(ip) && (ip == "::1" || ip.StartsWith("172.25") || ip.StartsWith("10.1"));
        }
    }
}
