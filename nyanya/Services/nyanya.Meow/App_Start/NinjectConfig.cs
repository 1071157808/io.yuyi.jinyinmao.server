// FileInformation: nyanya/nyanya.Meow/NinjectConfig.cs
// CreatedTime: 2014/09/17   11:18 AM
// LastUpdatedTime: 2014/09/18   2:21 PM

using Cat.Domain.Auth.Services;
using Cat.Domain.Auth.Services.Interfaces;
using Cat.Domain.Meow.Services;
using Cat.Domain.Meow.Services.Interfaces;
using Cat.Domain.Orders.Services;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Products.Services;
using Cat.Domain.Products.Services.Interfaces;
using Cat.Domain.Users.Services;
using Cat.Domain.Users.Services.Interfaces;
using Domian.Bus;
using Domian.Config;
using Infrastructure.SMS;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;
using System;
using System.Web;
using System.Web.Http;
using TimelineService = nyanya.Meow.Services.TimelineService;

namespace nyanya.Meow
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
            kernel.Bind<ISmsService>().To<SmsService>().InSingletonScope();
            kernel.Bind<IVeriCodeService>().To<VeriCodeService>().InSingletonScope();
            kernel.Bind<IBAProductInfoService>().To<BAProductInfoService>().InSingletonScope();
            kernel.Bind<ITAProductInfoService>().To<TAProductInfoService>().InSingletonScope();
            kernel.Bind<IProductInfoService>().To<ProductInfoService>().InSingletonScope();
            kernel.Bind<IExactProductInfoService>().To<ProductInfoService>().InSingletonScope();
            kernel.Bind<IProductService>().To<ProductService>().InSingletonScope();
            kernel.Bind<IBAOrderInfoService>().To<BAOrderInfoService>().InSingletonScope();
            kernel.Bind<ITAOrderInfoService>().To<TAOrderInfoService>().InSingletonScope();
            kernel.Bind<IOrderInfoService>().To<OrderInfoService>().InSingletonScope();
            kernel.Bind<IExactOrderInfoService>().To<OrderInfoService>().InSingletonScope();
            kernel.Bind<IUserInfoService>().To<UserInfoService>().InSingletonScope();
            kernel.Bind<IExactUserInfoService>().To<UserInfoService>().InSingletonScope();
            kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            kernel.Bind<ITimelineInfoService>().To<TimelineService>().InSingletonScope();
            kernel.Bind<IUpgradeService>().To<UpgradeService>().InSingletonScope();
            kernel.Bind<IFeedbackService>().To<FeedbackService>().InSingletonScope();
            kernel.Bind<Xingye.Domain.Orders.Services.Interfaces.IOrderInfoService>().To<Xingye.Domain.Orders.Services.OrderInfoService>().InSingletonScope();
            kernel.Bind<IParameterService>().To<ParameterService>().InSingletonScope();
            kernel.Bind<IZCBProductInfoService>().To<ZCBProductInfoService>().InSingletonScope();
            kernel.Bind<IZCBOrderService>().To<ZCBOrderService>().InSingletonScope();
        }
    }
}