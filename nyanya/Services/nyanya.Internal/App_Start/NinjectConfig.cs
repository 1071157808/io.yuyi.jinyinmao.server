// FileInformation: nyanya/nyanya.Internal/NinjectConfig.cs
// CreatedTime: 2014/08/26   1:28 PM
// LastUpdatedTime: 2014/09/01   4:32 PM

using Cat.Domain.Meow.Services;
using Cat.Domain.Meow.Services.Interfaces;
using Cat.Domain.Products.Services;
using Cat.Domain.Products.Services.Interfaces;
using Cat.Domain.Users.Services;
using Cat.Domain.Users.Services.Interfaces;
using Domian.Bus;
using Domian.Config;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using System;
using System.Web;
using System.Web.Http;

namespace nyanya.Internal.App_Start
{
    /// <summary>
    ///     Ninject Configuration
    /// </summary>
    public static class NinjectConfig
    {
        /// <summary>
        ///     RegisterDependencyResolver to HttpConfiguration
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        public static void RegisterDependencyResolver(HttpConfiguration config)
        {
            StandardKernel kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            // Configura Cqrs config
            CqrsConfiguration cqrsConfiguration = new CqrsConfiguration();
            CQRSConfig.Configurate(cqrsConfiguration);
            kernel.Bind<ICommandBus>().ToConstant(cqrsConfiguration.CommandBus).InSingletonScope();
            kernel.Bind<IEventBus>().ToConstant(cqrsConfiguration.EventBus).InSingletonScope();
            RegisterServices(kernel);

            // Configure Web API with the dependency resolver.
            config.DependencyResolver = new NinjectDependencyResolver(kernel);
        }

        /// <summary>
        ///     Registers the services.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IProductService>().To<ProductService>().InSingletonScope();
            kernel.Bind<IStatisticsService>().To<StatisticsService>().InSingletonScope();
            kernel.Bind<IZCBProductInfoService>().To<ZCBProductInfoService>().InSingletonScope();
        }
    }
}