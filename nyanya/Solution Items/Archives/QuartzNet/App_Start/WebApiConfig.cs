// FileInformation: nyanya/QuartzNet/WebApiConfig.cs
// CreatedTime: 2014/08/06   2:41 PM
// LastUpdatedTime: 2014/08/13   11:03 PM

using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using Services.WebAPI.Common.Filters;
using Services.WebAPI.Common.Providers;
using Services.WebAPI.Common.Services.ExceptionLoggers;

namespace QuartzNet
{
    /// <summary>
    ///     WebApiConfig
    /// </summary>
    public static class WebApiConfig
    {
        #region Public Methods

        /// <summary>
        ///     Register WebApiConfig
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Services.Replace(typeof(IFilterProvider), new ConfigurationFilterProvider());
            // Custom action filter provider which does ordering
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            config.Filters.Add(new NyanyaGlobalExceptionFilterAttribute());

            // There can be multiple exception loggers. (By default, no exception loggers are registered.)
            config.Services.Add(typeof(IExceptionLogger), new NlogTraceExceptionLogger());

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();
        }

        #endregion Public Methods
    }
}