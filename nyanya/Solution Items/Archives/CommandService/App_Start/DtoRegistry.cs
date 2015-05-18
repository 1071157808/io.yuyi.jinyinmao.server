// FileInformation: nyanya/CommandService/DtoRegistry.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/11   2:56 PM

using Cqrs.Commands.Order;
using Cqrs.Commands.Products;
using Cqrs.Commands.User;
using Cqrs.Domain.Commands;
using Cqrs.Domain.Config;
using Cqrs.Domain.Orders.CommandHandlers;
using Cqrs.Domain.Products.CommandHandlers;
using Cqrs.Domain.Users.CommandHandlers;
using Funq;
using ServiceStack.FluentValidation;
using ServiceStack.Validation;

namespace CommandService.App_Start
{
    /// <summary>
    ///     DtoRegistry
    /// </summary>
    public static class DtoRegistry
    {
        #region Public Methods

        /// <summary>
        ///     Registers the dtos.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="config">The configuration.</param>
        public static void RegisterDtos(Container container, CqrsConfiguration config)
        {
            UserCommandsHandler userCommandsHandler = new UserCommandsHandler(config);
            ProductCommandsHandler productCommandsHandler = new ProductCommandsHandler(config);
            OrderCommandsHandler orderCommandsHandler = new OrderCommandsHandler(config);
            DtoRegistryHelper helper = new DtoRegistryHelper(container, config);

            helper.RegisterDto<LaunchBAProduct, LaunchBAProductValidator>(productCommandsHandler);
            helper.RegisterDto<LaunchTAProduct, LaunchTAProductValidator>(productCommandsHandler);
            helper.RegisterDto<BAProductUnShelves, BAProductUnShelvesValidator>(productCommandsHandler);
            helper.RegisterDto<RegisterANewUser, RegisterANewUserValidator>(userCommandsHandler);
            helper.RegisterDto<SetPaymentPassword, SetPaymentPasswordValidator>(userCommandsHandler);
            helper.RegisterDto<SignUpPayment, SignUpPaymentValidator>(userCommandsHandler);
            helper.RegisterDto<AddBankCard, AddBankCardValidator>(userCommandsHandler);
            helper.RegisterDto<BuildInvestingOrder, BuildInvestingOrderValidator>(orderCommandsHandler);
            helper.RegisterDto<ChangeLoginPassword, ChangeLoginPasswordValidator>(userCommandsHandler);
            helper.RegisterDto<ProductRepay, ProductRepayValidator>(productCommandsHandler);
        }

        #endregion Public Methods
    }

    internal class DtoRegistryHelper
    {
        #region Private Fields

        private readonly CqrsConfiguration config;
        private readonly Container container;

        #endregion Private Fields

        #region Public Constructors

        public DtoRegistryHelper(Container container, CqrsConfiguration config)
        {
            this.container = container;
            this.config = config;
        }

        #endregion Public Constructors

        #region Internal Methods

        internal void RegisterDto<TCommand, TValidator>(params ICommandHandler<TCommand>[] handlers)
            where TCommand : Command
            where TValidator : AbstractValidator<TCommand>
        {
            this.container.RegisterValidator(typeof(TValidator));
            foreach (ICommandHandler<TCommand> handler in handlers)
            {
                this.config.CommandHandlers.Register(handler);
            }
        }

        internal void RegisterDto<TCommand>(params ICommandHandler<TCommand>[] handlers) where TCommand : Command
        {
            foreach (ICommandHandler<TCommand> handler in handlers)
            {
                this.config.CommandHandlers.Register(handler);
            }
        }

        #endregion Internal Methods
    }
}