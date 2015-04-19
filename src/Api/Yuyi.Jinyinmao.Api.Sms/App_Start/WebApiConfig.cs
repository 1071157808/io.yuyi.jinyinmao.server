// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-17  11:13 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  2:47 PM
// ***********************************************************************
// <copyright file="WebApiConfig.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using Moe.AspNet.Logs;
using Moe.AspNet.Providers;
using WebApiContrib.Formatting.Jsonp;

namespace Yuyi.Jinyinmao.Api.Sms
{
    /// <summary>
    ///     Class WebApiConfig.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            config.Services.Add(typeof(IExceptionLogger), new AzureExceptionLogger());

            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Info;

            config.AddJsonpFormatter();

            config.MapHttpAttributeRoutes();
        }
    }
}
