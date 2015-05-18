// FileInformation: nyanya/nyanya.AspDotNet.Common/NyanyaGlobalExceptionFilter.cs
// CreatedTime: 2014/09/01   11:07 AM
// LastUpdatedTime: 2014/09/01   11:11 AM

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Infrastructure.Lib.Exceptions;
using Infrastructure.Lib.Utility;

namespace nyanya.AspDotNet.Common.Filters
{
    public class NyanyaGlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        ///     Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            CommandExcuteFaildException commandExcuteFaildException = actionExecutedContext.Exception as CommandExcuteFaildException;

            if (commandExcuteFaildException != null)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, commandExcuteFaildException.FriendlyMessage);
            }

            HttpException httpException = actionExecutedContext.Exception as HttpException;
            if (httpException != null && httpException.Message.Contains("SSL"))
            {
                string location = "https://www.jinyinmao.com.cn";
                if (HttpUtils.IsMobileDevice(actionExecutedContext.Request))
                {
                    location = "https://m.jinyinmao.com.cn";
                }
                HttpResponseMessage response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.MovedPermanently);
                response.Headers.Location = new Uri(location);
                actionExecutedContext.Response = response;
            }
        }
    }
}