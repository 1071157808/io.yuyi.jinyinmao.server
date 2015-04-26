// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  1:04 AM
// ***********************************************************************
// <copyright file="EventProcessor.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Moe.Lib;
using Orleans;
using Yuyi.Jinyinmao.Service;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     EventProcessor.
    /// </summary>
    public abstract class EventProcessor<TEvent> where TEvent : IEvent
    {
        private readonly IEventProcessingLogger logger = new EventProcessingLogger();
        private readonly ISmsService smsService = new SmsService();

        /// <summary>
        ///     Gets the SMS service.
        /// </summary>
        /// <value>The SMS service.</value>
        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        protected ISmsService SmsService
        {
            get { return this.smsService; }
        }

        /// <summary>
        ///     Gets the error logger.
        /// </summary>
        /// <value>The error logger.</value>
        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        public IEventProcessingLogger ErrorLogger
        {
            get { return this.logger; }
        }

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public virtual Task ProcessEventAsync(TEvent @event)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    TopicClient client = TopicClient.CreateFromConnectionString(SiloClusterConfig.ServiceBusConnectiongString, @event.GetType().Name.ToUnderScope());
                    await client.SendAsync(new BrokeredMessage(@event));
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            });

            return TaskDone.Done;
        }
    }
}
