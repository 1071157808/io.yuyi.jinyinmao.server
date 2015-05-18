// FileInformation: nyanya/Services.WebAPI.Common/EmptyParameterFilterAttribute.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/07/28   2:43 AM

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Services.WebAPI.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EmptyParameterFilterAttribute : OrderedActionFilterAttribute
    {
        #region Public Constructors

        public EmptyParameterFilterAttribute(string parameterName)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException("parameterName");
            }

            this.ParameterName = parameterName;
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
                if (parameterValue == null)
                {
                    actionContext.ModelState.AddModelError(this.ParameterName, this.FormatErrorMessage(this.ParameterName));
                    //actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "非法参数");
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private string FormatErrorMessage(string parameterName)
        {
            return string.Format("The {0} cannot be null.", parameterName);
        }

        #endregion Private Methods
    }
}