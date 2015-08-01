// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : WebApiConfig.cs
// Created          : 2015-07-02  11:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-01  5:10 PM
// ***********************************************************************
// <copyright file="WebApiConfig.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Net.Http.Formatting;
using System.Web.Http;

namespace Yuyi.Jinyinmao.Proxy
{
    /// <summary>
    ///     WebApiConfig.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}