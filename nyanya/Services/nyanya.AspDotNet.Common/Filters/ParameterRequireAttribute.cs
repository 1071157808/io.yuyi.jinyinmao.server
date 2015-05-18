// FileInformation: nyanya/nyanya.AspDotNet.Common/ParameterRequireAttribute.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:13 AM

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace nyanya.AspDotNet.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ParameterRequireAttribute : OrderedActionFilterAttribute
    {
        public ParameterRequireAttribute(string parameterName, int order = 0)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            this.ParameterName = parameterName;
            this.Order = order;
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
                if (!this.IsValid(parameterValue))
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.FormatErrorMessage());
                }
            }
        }

        private string FormatErrorMessage()
        {
            return string.Format("{0}不能为空。", this.ParameterName);
        }

        private bool IsValid(object value)
        {
            return value != null && !String.IsNullOrWhiteSpace(value.ToString());
        }
    }
}