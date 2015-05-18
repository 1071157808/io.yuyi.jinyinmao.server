// FileInformation: nyanya/nyanya.Internal/WebApiConfig.cs
// CreatedTime: 2014/08/26   1:19 PM
// LastUpdatedTime: 2014/09/01   11:36 AM

using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using nyanya.AspDotNet.Common.Filters;
using nyanya.AspDotNet.Common.Providers;
using nyanya.AspDotNet.Common.Services.ExceptionLoggers;
using nyanya.AspDotNet.Common.Services.NLogTracing;

namespace nyanya.Internal.App_Start
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
            config.Services.Replace(typeof(IFilterProvider), new ConfigurationFilterProvider());
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            config.Filters.Add(new NyanyaGlobalExceptionFilterAttribute());

            config.Services.Replace(typeof(ITraceWriter), new NLogTraceWriter());
            config.Services.Add(typeof(IExceptionLogger), new NlogTraceExceptionLogger());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.MapHttpAttributeRoutes();
            // RegisterDependencyResolver
            NinjectConfig.RegisterDependencyResolver(config);
        }
    }
}