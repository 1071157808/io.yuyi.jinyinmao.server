// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:59 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-25  7:00 AM
// ***********************************************************************
// <copyright file="Global.asax.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.IO;
using System.Web;
using System.Web.Http;
using Orleans.Runtime.Host;

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
            //            Thread.Sleep(TimeSpan.FromSeconds(15));
            //            string configFilePath = Path.Combine(HttpRuntime.AppDomainAppPath, "bin", "ClientConfiguration.xml");
            //            GrainClient.Initialize(configFilePath);

            if (!AzureClient.IsInitialized)
            {
                FileInfo clientConfigFile = AzureConfigUtils.ClientConfigFileLocation;
                if (!clientConfigFile.Exists)
                {
                    throw new FileNotFoundException(string.Format("Cannot find Orleans client config file for initialization at {0}", clientConfigFile.FullName), clientConfigFile.FullName);
                }
                AzureClient.Initialize(clientConfigFile);
            }

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}