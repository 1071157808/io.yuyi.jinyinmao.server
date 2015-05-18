// FileInformation: nyanya/Services.WebAPI.V1.nyanya/Global.asax.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/29   3:56 PM

using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace Services.WebAPI.V1.nyanya
{
    /// <summary>
    ///     WebApiApplication
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        #region Protected Methods

        /// <summary>
        ///     Application_End
        /// </summary>
        protected void Application_End()
        {
            Logger logger = LogManager.GetLogger("ApplicationStartupLogger");
            if (logger == null)
            {
                throw new NLogConfigurationException("Can not find ApplicationStartupLogger");
            }
            logger.Info("Application Stopped.");
            logger.Info("==================================================================");
        }

        /// <summary>
        ///     Application_Start
        /// </summary>
        protected void Application_Start()
        {
            Logger logger = LogManager.GetLogger("ApplicationStartupLogger");
            if (logger == null)
            {
                throw new NLogConfigurationException("Can not find ApplicationStartupLogger");
            }
            try
            {
                logger.Info("==================================================================");
                logger.Info("Application Start");
                AreaRegistration.RegisterAllAreas();
                GlobalConfiguration.Configure(WebApiConfig.Register);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                MvcHandler.DisableMvcResponseHeader = true;
                logger.Info("------------------------------");
                logger.Info("Startup Successfully.");
                logger.Info("------------------------------");
                //new RedisClient();
            }
            catch (Exception e)
            {
                logger.Info("------------------------------");
                logger.Fatal(e.Message + e.InnerException.Message + e.StackTrace);
                logger.Info("------------------------------");
            }
        }

        #endregion Protected Methods
    }
}