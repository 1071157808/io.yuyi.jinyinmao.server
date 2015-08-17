// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : WebApiConfig.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  19:52
// ***********************************************************************
// <copyright file="WebApiConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using Moe.AspNet.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;
using ExceptionLogger = Moe.AspNet.Logs.ExceptionLogger;

namespace Yuyi.Jinyinmao.Api.Sms
{
    /// <summary>
    ///     Class WebApiConfig.
    /// </summary>
    internal static class WebApiConfig
    {
        /// <summary>
        ///     Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        internal static void Register(HttpConfiguration config)
        {
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            config.Services.Add(typeof(IExceptionLogger), new ExceptionLogger());

            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = false;
            traceWriter.MinimumLevel = TraceLevel.Error;

            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
                {
                    NullValueHandling = NullValueHandling.Include,
                    DateFormatString = "G",
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    Formatting = Formatting.None,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            config.Formatters.Clear();
            config.Formatters.Add(formatter);
            config.AddJsonpFormatter(formatter);

            config.MapHttpAttributeRoutes();
        }
    }
}