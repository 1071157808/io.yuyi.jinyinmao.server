// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : GlobalExceptionFilterAttribute.cs
// Created          : 2015-08-17  0:04
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  21:49
// ***********************************************************************
// <copyright file="GlobalExceptionFilterAttribute.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Yuyi.Jinyinmao.Api.Filters
{
    /// <summary>
    ///     GlobalExceptionFilterAttribute.
    /// </summary>
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        ///     Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                "出错了！请稍后再试", actionExecutedContext.Exception);
        }
    }
}