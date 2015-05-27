// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:04 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  3:53 PM
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
using Yuyi.Jinyinmao.Service;
using Yuyi.Jinyinmao.Service.Interface;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectConfig), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectConfig), "Stop")]

namespace Yuyi.Jinyinmao.Api
{
    /// <summary>
    ///     Ninject Configuration
    /// </summary>
    public static class NinjectConfig
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();
        private static readonly StandardKernel Kernel = new StandardKernel();

        /// <summary>
        ///     RegisterDependencyResolver to HttpConfiguration
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        public static void RegisterDependencyResolver(HttpConfiguration config) => config.DependencyResolver = new NinjectDependencyResolver(Kernel);

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop() => Bootstrapper.ShutDown();

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            Kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            Kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices();
            return Kernel;
        }

        /// <summary>
        ///     Registers the services.
        /// </summary>
        private static void RegisterServices()
        {
            // This is where we tell Ninject how to resolve service requests
            Kernel.Bind<IProductInfoService>().To<ProductInfoService>().InSingletonScope();
            Kernel.Bind<IProductService>().To<ProductService>().InSingletonScope();
            Kernel.Bind<ISmsService>().To<SmsService>().InSingletonScope();
            Kernel.Bind<IVeriCodeService>().To<VeriCodeService>().InSingletonScope();
            Kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            Kernel.Bind<IUserInfoService>().To<UserInfoService>().InSingletonScope()
                .WithConstructorArgument(new UserService());
        }
    }
}