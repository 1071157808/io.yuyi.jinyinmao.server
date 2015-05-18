// FileInformation: nyanya/nyanya.Internal/RouteConfig.cs
// CreatedTime: 2014/08/26   12:46 PM
// LastUpdatedTime: 2014/08/28   8:58 AM

using System.Web.Mvc;
using System.Web.Routing;

namespace nyanya.Internal
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
            routes.IgnoreRoute("CommandService/{*pathInfo}");
        }
    }
}