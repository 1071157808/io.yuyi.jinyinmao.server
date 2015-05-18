// FileInformation: nyanya/nyanya.Xingye/RouteConfig.cs
// CreatedTime: 2014/09/01   10:08 AM
// LastUpdatedTime: 2014/09/01   10:12 AM

using System.Web.Mvc;
using System.Web.Routing;

namespace nyanya.Xingye
{
    /// <summary>
    ///     RouteConfig
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        ///     Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }
    }
}