// FileInformation: nyanya/nyanya.Xingye.Internal/Global.asax.cs
// CreatedTime: 2014/09/03   9:48 AM
// LastUpdatedTime: 2014/09/03   5:47 PM

using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using nyanya.Xingye.Internal.App_Start;
using nyanya.Xingye.Internal.Service.Config;

namespace nyanya.Xingye.Internal
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
        }
    }
}