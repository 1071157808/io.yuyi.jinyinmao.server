// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  10:03 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  12:40 AM
// ***********************************************************************
// <copyright file="NinjectConfig.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Web;
using System.Web.Http;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using WebActivatorEx;
using Yuyi.Jinyinmao.Api;
using Yuyi.Jinyinmao.Service.Interface;
using Yuyi.Jinyinmao.Service.Misc.Interface;
using Yuyi.Jinyinmao.Services;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectConfig), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectConfig), "Stop")]

namespace Yuyi.Jinyinmao.Api
{
    /// <summary>
    ///     Ninject Configuration
    /// </summary>
    public static class NinjectConfig
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        private static readonly StandardKernel kernel = new StandardKernel();

        /// <summary>
        ///     RegisterDependencyResolver to HttpConfiguration
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        public static void RegisterDependencyResolver(HttpConfiguration config)
        {
            // Configure Web API with the dependency resolver.
            config.DependencyResolver = new NinjectDependencyResolver(kernel);
        }

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        ///     Registers the services.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // This is where we tell Ninject how to resolve service requests
            kernel.Bind<ISmsService>().To<SmsService>().InSingletonScope();
            kernel.Bind<IVeriCodeService>().To<VeriCodeService>().InSingletonScope();
        }
    }
}
