// FileInformation: nyanya/Services.WebAPI.V1.nyanya/RouteConfig.cs
// CreatedTime: 2014/06/03   3:35 PM
// LastUpdatedTime: 2014/06/03   3:42 PM

using System.Web.Mvc;
using System.Web.Routing;

namespace QuartzNet
{
    /// <summary>
    ///     RouteConfig
    /// </summary>
    public class RouteConfig
    {
        #region Public Methods

        /// <summary>
        ///     Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        #endregion Public Methods
    }
}