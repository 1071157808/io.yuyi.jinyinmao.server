// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  6:23 PM
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

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
            Thread.Sleep(10000);
            string configPaht = Path.Combine(HttpRuntime.AppDomainAppPath, "bin", "ClientConfiguration.xml");
            GrainClient.Initialize(configPaht);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
