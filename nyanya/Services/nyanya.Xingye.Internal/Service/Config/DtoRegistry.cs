// FileInformation: nyanya/nyanya.Xingye.Internal/DtoRegistry.cs
// CreatedTime: 2014/09/03   10:05 AM
// LastUpdatedTime: 2014/09/03   10:51 AM

using System;
using System.Collections.Generic;
using Domian.Commands;
using Domian.Config;
using Domian.Events;
using Funq;
using Infrastructure.SMS;
using ServiceStack.FluentValidation;
using ServiceStack.RabbitMq;
using ServiceStack.Validation;
using Xingye.Commands.Orders;
using Xingye.Commands.Products;
using Xingye.Commands.Users;
using Xingye.Domain.Meow.EventHandlers;
using Xingye.Domain.Orders.CommandHandlers;
using Xingye.Domain.Orders.EventHandlers;
using Xingye.Domain.Products.CommandHandlers;
using Xingye.Domain.Users.CommandHandlers;
using Xingye.Domain.Yilian.EventHandlers;
using Xingye.Events.Orders;
using Xingye.Events.Products;
using Xingye.Events.Users;
using Xingye.Events.Yilian;
using UserEventsHandler = Xingye.Domain.Users.EventHandlers.UserEventsHandler;
using YilianEventsHandler = Xingye.Domain.Yilian.EventHandlers.YilianEventsHandler;

namespace nyanya.Xingye.Internal.Service.Config
{
    /// <summary>
    ///     DtoRegistry
    /// </summary>
    public static class DtoRegistry
    {
        /// <summary>
        ///     Registers the dtos.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="config">The configuration.</param>
        public static void RegisterCommandDtos(Container container, CqrsConfiguration config)
        {
            UserCommandsHandler userCommandsHandler = new UserCommandsHandler(config);
            ProductCommandsHandler productCommandsHandler = new ProductCommandsHandler(config);
            OrderCommandsHandler orderCommandsHandler = new OrderCommandsHandler(config);
            CommandDtoRegistryHelper helper = new CommandDtoRegistryHelper(container, config);
            helper.RegisterDto<LaunchBAProduct, LaunchBAProductValidator>(productCommandsHandler);
            helper.RegisterDto<BAProductUnShelves, BAProductUnShelvesValidator>(productCommandsHandler);
            helper.RegisterDto<RegisterANewUser, RegisterANewUserValidator>(userCommandsHandler);
            helper.RegisterDto<TempRegisterANewUser, TempRegisterANewUserValidator>(userCommandsHandler);
            helper.RegisterDto<SetPaymentPassword, SetPaymentPasswordValidator>(userCommandsHandler);
            helper.RegisterDto<SignUpPayment, SignUpPaymentValidator>(userCommandsHandler);
            helper.RegisterDto<AddBankCard, AddBankCardValidator>(userCommandsHandler);
            helper.RegisterDto<BuildInvestingOrder, BuildInvestingOrderValidator>(orderCommandsHandler);
            helper.RegisterDto<ChangeLoginPassword, ChangeLoginPasswordValidator>(userCommandsHandler);
            helper.RegisterDto<ProductRepay, ProductRepayValidator>(productCommandsHandler);
        }

        /// <summary>
        ///     Registers the dtos.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="mqServer">The mq server.</param>
        /// <param name="container">The container.</param>
        /// <param name="config">The configuration.</param>
        public static void RegisterDtos(AppHost host, RabbitMqServer mqServer, Container container, CqrsConfiguration config)
        {
            YilianEventsHandler yilianEventsHandler = new YilianEventsHandler(config);
            UserEventsHandler userEventsHandler = new UserEventsHandler(config);
            OrderEventsHandler yilianOrderEventsHandler = new OrderEventsHandler(config);
            global::Xingye.Domain.Yilian.EventHandlers.UserEventsHandler yilianUserEventsHandler = new global::Xingye.Domain.Yilian.EventHandlers.UserEventsHandler(config);
            global::Xingye.Domain.Users.EventHandlers.YilianEventsHandler userYilianEventsHandler = new global::Xingye.Domain.Users.EventHandlers.YilianEventsHandler(config);
            global::Xingye.Domain.Orders.EventHandlers.YilianEventsHandler orderYilianEventsHandler = new global::Xingye.Domain.Orders.EventHandlers.YilianEventsHandler(config);
            global::Xingye.Domain.Products.EventHandlers.OrderEventsHandler productOrderEventsHandler = new global::Xingye.Domain.Products.EventHandlers.OrderEventsHandler(config);
            ProductEventsHandler orderProductEventsHandler = new ProductEventsHandler(config);
            SmsTriggerEventsHandler smsTriggerEventsHandler = new SmsTriggerEventsHandler(config, container.Resolve<ISmsService>());

            EventDtoRegistryHelper helper = new EventDtoRegistryHelper(host, mqServer, container, config);

            // Order
            helper.RegisterDto<OrderBuilded, OrderBuildedValidator>(yilianOrderEventsHandler);
            //helper.RegisterDto<OrderPaymentFailed, OrderPaymentFailedValidator>(productOrderEventsHandler);
            //helper.RegisterDto<OrderPaymentSuccessed, OrderPaymentSuccessedValidator>(productOrderEventsHandler);

            //Product
            helper.RegisterDto<ProductRepaid, ProductRepaidValidator>(orderProductEventsHandler);

            //User
            helper.RegisterDto<AppliedForAddBankCard, AppliedForAddBankCardValidator>(yilianUserEventsHandler);
            helper.RegisterDto<AppliedForSignUpPayment, AppliedForSignUpPaymentValidator>(yilianUserEventsHandler);

            //Yilian
            helper.RegisterDto<YilianAuthRequestSended, YilianAuthRequestSendedValidator>(userYilianEventsHandler);
            helper.RegisterDto<YilianAuthRequestCallbackReceived, YilianAuthRequestCallbackReceivedValidator>(yilianEventsHandler);
            helper.RegisterDto<YilianAuthRequestCallbackProcessed, YilianAuthRequestCallbackProcessedValidator>(userYilianEventsHandler);
            helper.RegisterDto<YilianQueryAuthRequestProcessed, YilianQueryAuthRequestProcessedValidator>(userYilianEventsHandler);
            helper.RegisterDto<YilianPaymentRequestSended, YilianPaymentRequestSendedValidator>(orderYilianEventsHandler);
            helper.RegisterDto<YilianPaymentRequestCallbackReceived, YilianPaymentRequestCallbackReceivedValidator>(yilianEventsHandler);
            helper.RegisterDto<YilianPaymentRequestCallbackProcessed, YilianPaymentRequestCallbackProcessedValidator>(orderYilianEventsHandler);
            helper.RegisterDto<YilianQueryPaymentRequestProcessed, YilianQueryPaymentRequestProcessedValidator>(orderYilianEventsHandler);

            //Sms
            helper.RegisterDto<RegisteredANewUser, RegisteredANewUserValidator>(smsTriggerEventsHandler);
            helper.RegisterDto<SignUpPaymentSucceeded, SignUpPaymentSucceededValidator>(smsTriggerEventsHandler);
            helper.RegisterDto<AddBankCardSucceeded, AddBankCardSucceededValidator>(smsTriggerEventsHandler);
            helper.RegisterDto<SignUpPaymentFailed, SignUpPaymentFailedValidator>(smsTriggerEventsHandler);
            helper.RegisterDto<AddBankCardFailed, AddBankCardFailedValidator>(smsTriggerEventsHandler);
            helper.RegisterDto<OrderPaymentSuccessed, OrderPaymentSuccessedValidator>(smsTriggerEventsHandler);
            helper.RegisterDto<OrderPaymentFailed, OrderPaymentFailedValidator>(smsTriggerEventsHandler);

            //
            helper.RegisterDto<UserSignInSucceeded, UserSignInSucceededValidator>(userEventsHandler);
        }
    }

    internal class CommandDtoRegistryHelper
    {
        private readonly CqrsConfiguration config;
        private readonly Container container;

        public CommandDtoRegistryHelper(Container container, CqrsConfiguration config)
        {
            this.container = container;
            this.config = config;
        }

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
    }

    internal class EventDtoRegistryHelper
    {
        private readonly CqrsConfiguration config;
        private readonly Container container;
        private readonly List<Type> eventTypes = new List<Type>();
        private readonly AppHost host;
        private readonly RabbitMqServer mqServer;
        private readonly List<Type> validatorTypes = new List<Type>();

        public EventDtoRegistryHelper(AppHost host, RabbitMqServer mqServer, Container container, CqrsConfiguration config)
        {
            this.host = host;
            this.mqServer = mqServer;
            this.container = container;
            this.config = config;
        }

        internal void RegisterDto<TEvent, TValidator>(params IEventHandler<TEvent>[] handlers)
            where TEvent : Event
            where TValidator : AbstractValidator<TEvent>
        {
            if (!this.eventTypes.Contains(typeof(TEvent)))
            {
                this.eventTypes.Add(typeof(TEvent));
                this.mqServer.RegisterHandler<TEvent>(this.host.ServiceController.ExecuteMessage, 3);
            }

            if (!this.validatorTypes.Contains(typeof(TValidator)))
            {
                this.validatorTypes.Add(typeof(TValidator));
                this.container.RegisterValidator(typeof(TValidator));
            }

            foreach (IEventHandler<TEvent> handler in handlers)
            {
                this.config.EventHandlers.Register(handler);
            }
        }

        internal void RegisterDto<TEvent>(params IEventHandler<TEvent>[] handlers) where TEvent : Event
        {
            if (!this.eventTypes.Contains(typeof(TEvent)))
            {
                this.eventTypes.Add(typeof(TEvent));
                this.mqServer.RegisterHandler<TEvent>(this.host.ServiceController.ExecuteMessage, 3);
            }

            foreach (IEventHandler<TEvent> handler in handlers)
            {
                this.config.EventHandlers.Register(handler);
            }
        }
    }
}