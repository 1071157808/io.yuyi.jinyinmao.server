// FileInformation: nyanya/nyanya.Cat/WebApiConfig.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/02   11:26 AM

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

namespace nyanya.Cat
{
    /// <summary>
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

            config.Services.Replace(typeof(IFilterProvider), new ConfigurationFilterProvider());
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            config.Filters.Add(new NyanyaGlobalExceptionFilterAttribute());

            config.Services.Replace(typeof(ITraceWriter), new NLogTraceWriter());
            config.Services.Add(typeof(IExceptionLogger), new NlogTraceExceptionLogger());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Routes.MapHttpBatchRoute("WebApiBatch", "$batch", new BatchHandler(GlobalConfiguration.DefaultServer));
            config.MapHttpAttributeRoutes();

            NinjectConfig.RegisterDependencyResolver(config);
        }
    }
}