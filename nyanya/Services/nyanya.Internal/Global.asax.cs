// FileInformation: nyanya/nyanya.Internal/Global.asax.cs
// CreatedTime: 2014/08/26   12:46 PM
// LastUpdatedTime: 2014/08/28   11:07 AM

using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using nyanya.Internal.App_Start;
using nyanya.Internal.Hangfire;
using nyanya.Internal.Service.Config;

namespace nyanya.Internal
{
    /// <summary>
    ///     WebApiApplication
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        ///     Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            (new AppHost()).Init();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
        }
    }
}