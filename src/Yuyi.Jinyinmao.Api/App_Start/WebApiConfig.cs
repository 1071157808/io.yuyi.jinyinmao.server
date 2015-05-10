// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:59 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  11:48 AM
// ***********************************************************************
// <copyright file="WebApiConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using Moe.AspNet.Logs;
using Moe.AspNet.MessageHandlers;
using Moe.AspNet.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;

namespace Yuyi.Jinyinmao.Api
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

            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            formatter.SerializerSettings.DateFormatString = "G";
            formatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Populate;
            formatter.SerializerSettings.Formatting = Formatting.None;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Clear();
            config.Formatters.Add(formatter);
            config.AddJsonpFormatter(formatter);

            config.Routes.MapHttpBatchRoute("WebApiBatch", "$batch", new BatchHandler(GlobalConfiguration.DefaultServer));
            config.MapHttpAttributeRoutes();

            config.EnableCors(new EnableCorsAttribute("http://www.jinyinmao.com.cn,https://www.jinyinmao.com.cn", "*", "*"));

            NinjectConfig.RegisterDependencyResolver(config);
        }
    }
}