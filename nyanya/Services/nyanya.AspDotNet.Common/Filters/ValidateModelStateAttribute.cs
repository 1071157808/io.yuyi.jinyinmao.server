// FileInformation: nyanya/nyanya.AspDotNet.Common/ValidateModelStateAttribute.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:13 AM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Infrastructure.Lib.Extensions;

namespace nyanya.AspDotNet.Common.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateModelStateAttribute : OrderedActionFilterAttribute
    {
        /// <summary>
        ///     Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                KeyValuePair<string, ModelState> error = actionContext.ModelState.FirstOrDefault(s => s.Value.Errors.Count > 0);
                ModelError modelError = error.Value.Errors.FirstOrDefault();
                string errorMessage = modelError != null && modelError.ErrorMessage.IsNotNullOrEmpty() ? modelError.ErrorMessage : error.Key + "参数验证错误";
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage);
            }
        }
    }
}