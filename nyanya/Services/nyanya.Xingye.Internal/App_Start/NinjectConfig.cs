// FileInformation: nyanya/nyanya.Xingye.Internal/NinjectConfig.cs
// CreatedTime: 2014/08/26   1:28 PM
// LastUpdatedTime: 2014/09/01   4:32 PM

using Domian.Bus;
using Domian.Config;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using System;
using System.Web;
using System.Web.Http;
using Xingye.Domain.Products.Services;
using Xingye.Domain.Products.Services.Interfaces;
using Xingye.Domain.Users.Services;
using Xingye.Domain.Users.Services.Interfaces;

namespace nyanya.Xingye.Internal.App_Start
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
        }
    }
}