// FileInformation: nyanya/nyanya.Cat/RouteConfig.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   10:12 AM

using System.Web.Mvc;
using System.Web.Routing;

namespace nyanya.Cat
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