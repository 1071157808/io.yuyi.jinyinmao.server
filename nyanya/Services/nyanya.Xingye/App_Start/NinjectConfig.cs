// FileInformation: nyanya/nyanya.Xingye/NinjectConfig.cs
// CreatedTime: 2014/09/01   10:08 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using Domian.Bus;
using Domian.Config;
using Infrastructure.SMS;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using System;
using System.Web;
using System.Web.Http;
using Xingye.Domain.Auth.Services;
using Xingye.Domain.Auth.Services.Interfaces;
using Xingye.Domain.Orders.Services;
using Xingye.Domain.Orders.Services.Interfaces;
using Xingye.Domain.Products.Services;
using Xingye.Domain.Products.Services.Interfaces;
using Xingye.Domain.Users.Services;
using Xingye.Domain.Users.Services.Interfaces;

namespace nyanya.Xingye
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
            // This is where we tell Ninject how to resolve service requests
            // v2
            kernel.Bind<ISmsService>().To<SmsService>().InSingletonScope();
            kernel.Bind<IVeriCodeService>().To<VeriCodeService>().InSingletonScope();
            kernel.Bind<IBAProductInfoService>().To<BAProductInfoService>().InSingletonScope();
            kernel.Bind<ITAProductInfoService>().To<TAProductInfoService>().InSingletonScope();
            kernel.Bind<IProductInfoService>().To<ProductInfoService>().InSingletonScope();
            kernel.Bind<IExactProductInfoService>().To<ProductInfoService>().InSingletonScope();
            kernel.Bind<IProductService>().To<ProductService>().InSingletonScope();
            kernel.Bind<IBAOrderInfoService>().To<BAOrderInfoService>().InSingletonScope();
            kernel.Bind<ITAOrderInfoService>().To<TAOrderInfoService>().InSingletonScope();
            kernel.Bind<IXYOrderInfoService>().To<XYOrderInfoService>().InSingletonScope();
            kernel.Bind<IOrderInfoService>().To<OrderInfoService>().InSingletonScope();
            kernel.Bind<IUserInfoService>().To<UserInfoService>().InSingletonScope();
            kernel.Bind<IExactUserInfoService>().To<UserInfoService>().InSingletonScope();
            kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
        }
    }
}