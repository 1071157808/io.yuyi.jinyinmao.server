// FileInformation: nyanya/nyanya.Meow/RouteConfig.cs
// CreatedTime: 2014/08/28   6:33 PM
// LastUpdatedTime: 2014/08/29   2:12 PM

using System.Web.Mvc;
using System.Web.Routing;

namespace nyanya.Meow
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