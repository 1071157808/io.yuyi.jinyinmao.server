// FileInformation: nyanya/nyanya.AspDotNet.Common/ParameterCellphoneFormatAttribute.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:12 AM

using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using Infrastructure.Lib.Utility;

namespace nyanya.AspDotNet.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ParameterCellphoneFormatAttribute : OrderedActionFilterAttribute
    {
        private readonly Regex regex = RegexUtils.CellphoneRegex;

        public ParameterCellphoneFormatAttribute(string parameterName, int order = 0)
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
            return string.Format("手机号格式不正确。");
        }

        private bool IsValid(object value)
        {
            // Convert the value to a string
            string stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);

            // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
            if (String.IsNullOrEmpty(stringValue)) return true;

            Match m = this.regex.Match(stringValue);

            // We are looking for an exact match, not just a search hit. This matches what
            // the RegularExpressionValidator control does
            return (m.Success && m.Index == 0 && m.Length == stringValue.Length);
        }
    }
}