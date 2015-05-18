// FileInformation: nyanya/nyanya.Meow/WebApiConfig.cs
// CreatedTime: 2014/08/28   6:33 PM
// LastUpdatedTime: 2014/09/01   10:13 AM

using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using nyanya.AspDotNet.Common.Filters;
using nyanya.AspDotNet.Common.MessageHandlers;
using nyanya.AspDotNet.Common.Providers;
using nyanya.AspDotNet.Common.RequestCommands;
using nyanya.AspDotNet.Common.Services.ExceptionLoggers;
using nyanya.AspDotNet.Common.Services.NLogTracing;

namespace nyanya.Meow
{
    /// <summary>
    ///     WebApiConfig
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            config.ParameterBindingRules.Insert(0,
                descriptor => typeof(IRequestCommand).IsAssignableFrom(descriptor.ParameterType)
                    ? new FromUriAttribute().GetBinding(descriptor) : null);

            // order fundation
            config.Services.Replace(typeof(IFilterProvider), new ConfigurationFilterProvider());
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());

            // 全局异常处理
            config.Filters.Add(new NyanyaGlobalExceptionFilterAttribute());

            // trace logger
            config.Services.Replace(typeof(ITraceWriter), new NLogTraceWriter());

            // exception logger
            config.Services.Add(typeof(IExceptionLogger), new NlogTraceExceptionLogger());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Routes.MapHttpBatchRoute("WebApiBatch", "$batch", new BatchHandler(GlobalConfiguration.DefaultServer));
            config.MapHttpAttributeRoutes();

            NinjectConfig.RegisterDependencyResolver(config);
        }
    }
}