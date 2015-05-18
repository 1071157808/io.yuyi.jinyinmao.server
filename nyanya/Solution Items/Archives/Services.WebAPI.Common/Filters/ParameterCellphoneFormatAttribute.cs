// FileInformation: nyanya/Services.WebAPI.Common/ParameterCellphoneFormatAttribute.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/13   10:05 PM

using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using Infrastructure.Lib.Utility;

namespace Services.WebAPI.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ParameterCellphoneFormatAttribute : OrderedActionFilterAttribute
    {
        #region Private Fields

        private readonly Regex regex = RegexUtils.CellphoneRegex;

        #endregion Private Fields

        #region Public Constructors

        public ParameterCellphoneFormatAttribute(string parameterName, int order = 0)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            this.ParameterName = parameterName;
            this.Order = order;
        }

        #endregion Public Constructors

        #region Public Properties

        public string ParameterName { get; private set; }

        #endregion Public Properties

        #region Public Methods

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

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}