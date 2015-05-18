// FileInformation: nyanya/Services.WebAPI.Common/RangeFilterAttribute.cs
// CreatedTime: 2014/04/02   5:53 PM
// LastUpdatedTime: 2014/04/03   3:58 PM

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Services.WebAPI.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RangeFilterAttribute : OrderedActionFilterAttribute
    {
        public RangeFilterAttribute(string parameterName, int min = Int32.MinValue, int max = Int32.MaxValue, int order = 0)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            this.ParameterName = parameterName;
            this.MinValue = min;
            this.MaxValue = max;
            this.Order = order;
        }

        public int MaxValue { get; private set; }

        public int MinValue { get; private set; }

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
                if (parameterValue == null || !this.IsValid(parameterValue))
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.FormatErrorMessage());
                }
            }
        }

        private string FormatErrorMessage()
        {
            return string.Format("{0}范围不合法。", this.ParameterName);
        }

        private bool IsValid(object value)
        {
            int i;
            if (int.TryParse(value.ToString(), out i))
            {
                return i >= this.MinValue && i <= this.MaxValue;
            }
            return false;
        }
    }
}