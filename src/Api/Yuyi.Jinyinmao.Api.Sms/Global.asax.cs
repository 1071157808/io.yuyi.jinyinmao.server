// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-17  11:13 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-19  12:38 AM
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Web;
using System.Web.Http;

namespace Yuyi.Jinyinmao.Api.Sms
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
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
