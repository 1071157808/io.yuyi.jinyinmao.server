// FileInformation: nyanya/EventService/DtoRegistry.cs
// CreatedTime: 2014/08/13   1:18 AM
// LastUpdatedTime: 2014/08/17   3:19 PM

using System;
using System.Collections.Generic;
using Cqrs.Domain.Config;
using Cqrs.Domain.Events;
using Cqrs.Domain.Meow.EventHandlers;
using Cqrs.Domain.Orders.EventHandlers;
using Cqrs.Domain.Yilian.EventHandlers;
using Cqrs.Events.Orders;
using Cqrs.Events.Products;
using Cqrs.Events.User;
using Cqrs.Events.Yilian;
using Funq;
using Infrastructure.SMS;
using ServiceStack.FluentValidation;
using ServiceStack.RabbitMq;
using ServiceStack.Validation;
using YilianEventsHandler = Cqrs.Domain.Yilian.EventHandlers.YilianEventsHandler;

namespace EventService.App_Start
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
        /// <param name="host">The host.</param>
        /// <param name="mqServer">The mq server.</param>
        /// <param name="container">The container.</param>
        /// <param name="config">The configuration.</param>
        public static void RegisterDtos(AppHost host, RabbitMqServer mqServer, Container container, CqrsConfiguration config)
        {
            YilianEventsHandler yilianEventsHandler = new YilianEventsHandler(config);
            OrderEventsHandler yilianOrderEventsHandler = new OrderEventsHandler(config);
            UserEventsHandler yilianUserEventsHandler = new UserEventsHandler(config);
            Cqrs.Domain.Users.EventHandlers.UserEventsHandler userEventsHandler = new Cqrs.Domain.Users.EventHandlers.UserEventsHandler(config);
            Cqrs.Domain.Users.EventHandlers.YilianEventsHandler userYilianEventsHandler = new Cqrs.Domain.Users.EventHandlers.YilianEventsHandler(config);
            Cqrs.Domain.Orders.EventHandlers.YilianEventsHandler orderYilianEventsHandler = new Cqrs.Domain.Orders.EventHandlers.YilianEventsHandler(config);
            Cqrs.Domain.Products.EventHandlers.OrderEventsHandler productOrderEventsHandler = new Cqrs.Domain.Products.EventHandlers.OrderEventsHandler(config);
            ProductEventsHandler orderProductEventsHandler = new ProductEventsHandler(config);
            SmsTriggerEventsHandler smsTriggerEventsHandler = new SmsTriggerEventsHandler(config, container.Resolve<ISmsService>());

            DtoRegistryHelper helper = new DtoRegistryHelper(host, mqServer, container, config);

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

        #endregion Public Methods
    }

    internal class DtoRegistryHelper
    {
        #region Private Fields

        private readonly CqrsConfiguration config;
        private readonly Container container;
        private readonly List<Type> eventTypes = new List<Type>();
        private readonly AppHost host;
        private readonly RabbitMqServer mqServer;
        private readonly List<Type> validatorTypes = new List<Type>();

        #endregion Private Fields

        #region Public Constructors

        public DtoRegistryHelper(AppHost host, RabbitMqServer mqServer, Container container, CqrsConfiguration config)
        {
            this.host = host;
            this.mqServer = mqServer;
            this.container = container;
            this.config = config;
        }

        #endregion Public Constructors

        #region Internal Methods

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

        #endregion Internal Methods
    }
}