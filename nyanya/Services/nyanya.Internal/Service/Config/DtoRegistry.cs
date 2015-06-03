// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-05-18  2:55 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-03  11:57 AM
// ***********************************************************************
// <copyright file="DtoRegistry.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Cat.Commands.Orders;
using Cat.Commands.Products;
using Cat.Commands.Users;
using Cat.Domain.Meow.EventHandlers;
using Cat.Domain.Orders.CommandHandlers;
using Cat.Domain.Products.CommandHandlers;
using Cat.Domain.Products.EventHandlers;
using Cat.Domain.Users.CommandHandlers;
using Cat.Domain.Yilian.EventHandlers;
using Cat.Events.Orders;
using Cat.Events.Products;
using Cat.Events.Users;
using Cat.Events.Yilian;
using Domian.Commands;
using Domian.Config;
using Domian.Events;
using Funq;
using Infrastructure.SMS;
using ServiceStack.FluentValidation;
using ServiceStack.RabbitMq;
using ServiceStack.Validation;
using C = Cat;
using OrderEventsHandler = Cat.Domain.Yilian.EventHandlers.OrderEventsHandler;
using UserEventsHandler = Cat.Domain.Users.EventHandlers.UserEventsHandler;

namespace nyanya.Internal.Service.Config
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
            helper.RegisterDto<LaunchTAProduct, LaunchTAProductValidator>(productCommandsHandler);
            helper.RegisterDto<BAProductUnShelves, BAProductUnShelvesValidator>(productCommandsHandler);
            helper.RegisterDto<RegisterANewUser, RegisterANewUserValidator>(userCommandsHandler);
            helper.RegisterDto<SetPaymentPassword, SetPaymentPasswordValidator>(userCommandsHandler);
            helper.RegisterDto<SignUpPayment, SignUpPaymentValidator>(userCommandsHandler);
            helper.RegisterDto<AddBankCard, AddBankCardValidator>(userCommandsHandler);
            helper.RegisterDto<BuildInvestingOrder, BuildInvestingOrderValidator>(orderCommandsHandler);
            helper.RegisterDto<ChangeLoginPassword, ChangeLoginPasswordValidator>(userCommandsHandler);
            helper.RegisterDto<ProductRepay, ProductRepayValidator>(productCommandsHandler);
            helper.RegisterDto<LaunchZCBProduct, LaunchZCBProductValidator>(productCommandsHandler);
            helper.RegisterDto<ZCBUpdateShareCount, ZCBUpdateShareCountValidator>(productCommandsHandler);
            helper.RegisterDto<BuildRedeemPrincipal, BuildRedeemPrincipalValidator>(orderCommandsHandler);
            helper.RegisterDto<SetZCBRedeemBillsResult, SetZCBRedeemBillsResultValidator>(orderCommandsHandler);
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
            global::Cat.Domain.Yilian.EventHandlers.UserEventsHandler yilianUserEventsHandler = new global::Cat.Domain.Yilian.EventHandlers.UserEventsHandler(config);
            global::Cat.Domain.Users.EventHandlers.YilianEventsHandler userYilianEventsHandler = new global::Cat.Domain.Users.EventHandlers.YilianEventsHandler(config);
            global::Cat.Domain.Orders.EventHandlers.YilianEventsHandler orderYilianEventsHandler = new global::Cat.Domain.Orders.EventHandlers.YilianEventsHandler(config);
            global::Cat.Domain.Products.EventHandlers.OrderEventsHandler productOrderEventsHandler = new global::Cat.Domain.Products.EventHandlers.OrderEventsHandler(config);
            ProductEventsHandler productEventsHandler = new ProductEventsHandler(config);
            global::Cat.Domain.Orders.EventHandlers.ProductEventsHandler orderProductEventsHandler = new global::Cat.Domain.Orders.EventHandlers.ProductEventsHandler(config);
            SmsTriggerEventsHandler smsTriggerEventsHandler = new SmsTriggerEventsHandler(config, container.Resolve<ISmsService>());
            EventDtoRegistryHelper helper = new EventDtoRegistryHelper(host, mqServer, container, config);

            // Order
            helper.RegisterDto<OrderBuilded, OrderBuildedValidator>(yilianOrderEventsHandler);
            //helper.RegisterDto<OrderPaymentFailed, OrderPaymentFailedValidator>(productOrderEventsHandler);
            //helper.RegisterDto<OrderPaymentSuccessed, OrderPaymentSuccessedValidator>(productOrderEventsHandler);

            //Product
            helper.RegisterDto<ProductRepaid, ProductRepaidValidator>(orderProductEventsHandler);
            helper.RegisterDto<ZCBUpdateShareCounted, ZCBUpdateShareCountedValidator>(productEventsHandler);

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
            helper.RegisterDto<SetRedeemBillResult, SetRedeemBillResultValidator>(smsTriggerEventsHandler);

            //
            helper.RegisterDto<UserSignInSucceeded, UserSignInSucceededValidator>(userEventsHandler);
            helper.RegisterDto<RegisteredANewUser, RegisteredANewUserValidator>(userEventsHandler);
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
                this.mqServer.RegisterHandler<TEvent>(this.host.ServiceController.ExecuteMessage, 100);
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
                this.mqServer.RegisterHandler<TEvent>(this.host.ServiceController.ExecuteMessage, 15);
            }

            foreach (IEventHandler<TEvent> handler in handlers)
            {
                this.config.EventHandlers.Register(handler);
            }
        }
    }
}