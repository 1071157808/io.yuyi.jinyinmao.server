// FileInformation: nyanya/Services.WebAPI.V1.nyanya/NinjectConfig.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/11   2:34 PM

using System;
using System.Web;
using System.Web.Http;
using Cqrs.Domain.Auth.Services;
using Cqrs.Domain.Auth.Services.Interfaces;
using Cqrs.Domain.Bus;
using Cqrs.Domain.Config;
using Cqrs.Domain.Orders.Services;
using Cqrs.Domain.Orders.Services.Interfaces;
using Cqrs.Domain.Products.Services;
using Cqrs.Domain.Products.Services.Interfaces;
using Cqrs.Domain.Users.Services;
using Cqrs.Domain.Users.Services.Interfaces;
using Infrastructure.SMS;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.WebApi;

namespace Services.WebAPI.V1.nyanya.App_Start
{
    /// <summary>
    ///     Ninject Configuration
    /// </summary>
    public static class NinjectConfig
    {
        #region Public Methods

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

        #endregion Public Methods

        #region Private Methods

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
            kernel.Bind<IOrderInfoService>().To<OrderInfoService>().InSingletonScope();
            kernel.Bind<IUserInfoService>().To<UserInfoService>().InSingletonScope();
            kernel.Bind<IExactUserInfoService>().To<UserInfoService>().InSingletonScope();
            kernel.Bind<IUserService>().To<UserService>().InSingletonScope();

            // v1
            //kernel.Bind<IAuthenticationService>().To<AuthenticationService>().InSingletonScope();
            //kernel.Bind<IInvestmentStatisticService>().To<InvestmentStatisticService>().InSingletonScope();
            //kernel.Bind<IItemService>().To<ItemService>().InSingletonScope();
            //kernel.Bind<IMeowConfigurationService>().To<MeowConfigurationService>().InSingletonScope();
            //kernel.Bind<IOrderService>().To<OrderService>().InSingletonScope();
            //kernel.Bind<IOrderSummaryService>().To<OrderSummaryService>().InSingletonScope();
            //kernel.Bind<IProductService>().To<ProductService>().InSingletonScope();
            //kernel.Bind<ITimelineService>().To<TimelineService>().InSingletonScope();
            //kernel.Bind<IUserInfoService>().To<UserInfoService>().InSingletonScope();
            //kernel.Bind<IUserService>().To<UserService>().InSingletonScope();
            //kernel.Bind<IUserProfileDataService>().To<UserProfileCacheService>().InSingletonScope().WithConstructorArgument("userProfileDataService", kernel.Get<UserProfileDataService>());
        }

        #endregion Private Methods
    }
}