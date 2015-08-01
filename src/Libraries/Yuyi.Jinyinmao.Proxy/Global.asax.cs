// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Global.asax.cs
// Created          : 2015-07-02  11:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-01  5:15 PM
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web;
using System.Web.Http;

namespace Yuyi.Jinyinmao.Proxy
{
    /// <summary>
    ///     WebApiApplication.
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        ///     Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}