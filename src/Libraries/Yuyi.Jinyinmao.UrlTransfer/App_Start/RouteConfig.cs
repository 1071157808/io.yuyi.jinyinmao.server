// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : RouteConfig.cs
// Created          : 2015-08-17  12:49
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  15:49
// ***********************************************************************
// <copyright file="RouteConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web.Mvc;
using System.Web.Routing;

namespace Yuyi.Jinyinmao.UrlTransfer
{
    /// <summary>
    ///     RouteConfig.
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

            routes.MapMvcAttributeRoutes();
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}