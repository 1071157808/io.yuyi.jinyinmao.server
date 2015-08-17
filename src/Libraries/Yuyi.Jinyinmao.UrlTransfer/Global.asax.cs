// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Global.asax.cs
// Created          : 2015-08-17  12:49
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  13:21
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Yuyi.Jinyinmao.UrlTransfer
{
    /// <summary>
    ///     MvcApplication.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        ///     Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}