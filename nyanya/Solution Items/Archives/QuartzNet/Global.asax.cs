// FileInformation: nyanya/Services.WebAPI.V1.nyanya/Global.asax.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/20   11:40 AM

using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QuartzNet
{
    /// <summary>
    ///     WebApiApplication
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        #region Protected Methods

        /// <summary>
        ///     Application_Start
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SchedulerConfig.RegisterScheduler();
        }

        #endregion Protected Methods
    }
}