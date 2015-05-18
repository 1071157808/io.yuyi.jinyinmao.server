// FileInformation: nyanya/Services.WebAPI.V1.nyanya/WebApiConfig.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/27   12:42 PM

using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.Common.MessageHandlers;
using Services.WebAPI.Common.Providers;
using Services.WebAPI.Common.RequestCommands;
using Services.WebAPI.Common.Services.ExceptionLoggers;
using Services.WebAPI.Common.Services.NLogTracing;
using Services.WebAPI.V1.nyanya.App_Start;

namespace Services.WebAPI.V1.nyanya
{
    /// <summary>
    ///     WebApiConfig
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Register WebApiConfig
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Any complex type parameter which is assignable From
            // IRequestCommand will be bound from the URI.
            // Pro ASP.NET Web API: HTTP Web Services in ASP.NET By Tugberk Ugurlu, Alexander Zeitler  [p.160]
            config.ParameterBindingRules.Insert(0,
                descriptor => typeof(IRequestCommand).IsAssignableFrom(descriptor.ParameterType)
                    ? new FromUriAttribute().GetBinding(descriptor) : null);

            // Start clean by replacing with filter provider for global configuration.
            // For these globally added filters we need not do any ordering as filters are
            // executed in the order they are added to the filter collection
            config.Services.Replace(typeof(IFilterProvider), new ConfigurationFilterProvider());
            // Custom action filter provider which does ordering
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            config.Filters.Add(new NyanyaGlobalExceptionFilterAttribute());
            // http://www.asp.net/web-api/overview/web-api-routing-and-actions/exception-handling
            // By default, most exceptions are translated into an HTTP response with status code 500, Internal Server Error.
            // The HttpResponseException type is a special case. This exception returns any HTTP status code that you specify in the exception constructor.
            // There must be exactly one exception handler. (There is a default one that may be replaced.)
            // To make this sample easier to run in a browser, replace the default exception handler with one that sends
            // back 500 empty response for all errors.
            // config.Services.Replace(typeof(IExceptionHandler), new GenericGlobalExceptionHandler());

            config.Services.Replace(typeof(ITraceWriter), new NLogTraceWriter());
            // There can be multiple exception loggers. (By default, no exception loggers are registered.)
            config.Services.Add(typeof(IExceptionLogger), new NlogTraceExceptionLogger());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.Routes.MapHttpBatchRoute("WebApiBatch", "$batch", new BatchHandler(GlobalConfiguration.DefaultServer));
            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpBatchRoute("WebApiJsonBatch", "$batch", new JsonBatchHandler(GlobalConfiguration.DefaultServer));
            //config.Routes.MapHttpRoute("RPCApi", "{controller}/{action}", new { action = "Status" }, new { controller = "^([Ss]tatus|[Dd]ev[Hh]elper)$" });
            // config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });

            // RegisterDependencyResolver
            NinjectConfig.RegisterDependencyResolver(config);

            // RegisterMessageHandlers
            //config.MessageHandlers.Add(new ServerTimeHandler());
        }
    }
}