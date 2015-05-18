// FileInformation: nyanya/nyanya.AspDotNet.Common/EmptyParameterFilterAttribute.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:11 AM

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace nyanya.AspDotNet.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EmptyParameterFilterAttribute : OrderedActionFilterAttribute
    {
        public EmptyParameterFilterAttribute(string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            this.ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }

        /// <summary>
        ///     Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            object parameterValue;
            if (actionContext.ActionArguments.TryGetValue(this.ParameterName, out parameterValue))
            {
                if (parameterValue == null)
                {
                    actionContext.ModelState.AddModelError(this.ParameterName, this.FormatErrorMessage(this.ParameterName));
                    //actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "非法参数");
                }
            }
        }

        private string FormatErrorMessage(string parameterName)
        {
            return string.Format("The {0} cannot be null.", parameterName);
        }
    }
}