// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : HttpConfigurationExtensions.cs
// Created          : 2015-08-16  22:15
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-16  22:55
// ***********************************************************************
// <copyright file="HttpConfigurationExtensions.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Tracing;

namespace Yuyi.Jinyinmao.Log
{
    /// <summary>
    ///     HttpConfigurationExtensions.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        ///     Uses the nlog.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNLog(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(ITraceWriter), new NLogTraceWriter());
            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());
            return config;
        }

        /// <summary>
        ///     Uses the nlog exception logger.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNLogExceptionLogger(this HttpConfiguration config)
        {
            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());
            return config;
        }

        /// <summary>
        ///     Uses the nlog trace writer.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseNLogTraceWriter(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(ITraceWriter), new NLogTraceWriter());
            return config;
        }
    }
}