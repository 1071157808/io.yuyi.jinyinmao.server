// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:59 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-20  12:13 AM
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Http;
using Orleans;

namespace Yuyi.Jinyinmao.Api
{
    /// <summary>
    ///     Class WebApiApplication.
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        ///     Application start.
        /// </summary>
        protected void Application_Start()
        {
            Thread.Sleep(TimeSpan.FromSeconds(15));
            string configFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, "bin", "ClientConfiguration.xml");
            GrainClient.Initialize(configFilePath);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}