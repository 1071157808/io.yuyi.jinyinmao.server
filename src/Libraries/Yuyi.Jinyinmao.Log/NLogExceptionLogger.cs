// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : NLogExceptionLogger.cs
// Created          : 2015-08-16  22:53
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-16  22:54
// ***********************************************************************
// <copyright file="NLogExceptionLogger.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Moe.AspNet.Utility;
using Moe.Lib;
using NLog;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     NLogExceptionLogger.
    /// </summary>
    public sealed class NLogExceptionLogger : ExceptionLogger
    {
        /// <summary>
        ///     The logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NLogExceptionLogger" /> class.
        /// </summary>
        /// <exception cref="NLog.NLogConfigurationException">Can not find ExceptionLogger</exception>
        public NLogExceptionLogger()
        {
            this.logger = LogManager.GetExceptionLogger();
        }

        /// <summary>
        ///     When overridden in a derived class, logs the exception synchronously.
        /// </summary>
        /// <param name="context">The exception logger context.</param>
        public override void Log(ExceptionLoggerContext context)
        {
            try
            {
                // Retrieve the current HttpContext instance for this request.
                HttpContext httpContext = context.Request.ToHttpContext();

                if (httpContext == null)
                {
                    return;
                }

                string request = httpContext.Request.Dump();

                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.AppendFormat("{0} {1} {2} {3}", httpContext.Request.HttpMethod, httpContext.Request.RawUrl, httpContext.Response.StatusCode, httpContext.Response.Status);
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append(request);
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append(context.Exception.GetExceptionString());

                this.logger.Error(messageBuilder.ToString());
            }
            catch (Exception e)
            {
                this.logger.Fatal("NLogExceptionLoggerInternalError" + e);
            }
        }
    }
}