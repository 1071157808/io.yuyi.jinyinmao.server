// FileInformation: nyanya/nyanya.Internal/IpAuthorize.cs
// CreatedTime: 2014/08/26   5:00 PM
// LastUpdatedTime: 2014/09/01   11:36 AM

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using Infrastructure.Lib.Utility;
using nyanya.AspDotNet.Common.Filters;

namespace nyanya.Internal.Filters
{
    /// <summary>
    ///     TokenAuthorizeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
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