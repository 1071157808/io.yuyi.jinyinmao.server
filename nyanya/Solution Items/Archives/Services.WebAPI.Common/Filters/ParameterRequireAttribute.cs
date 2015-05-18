// FileInformation: nyanya/Services.WebAPI.Common/ParameterRequireAttribute.cs
// CreatedTime: 2014/04/02   3:54 PM
// LastUpdatedTime: 2014/04/03   3:58 PM

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Services.WebAPI.Common.Filters
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