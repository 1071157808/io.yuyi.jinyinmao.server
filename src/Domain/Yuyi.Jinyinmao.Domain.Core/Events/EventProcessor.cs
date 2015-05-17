// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-15  9:52 PM
// ***********************************************************************
// <copyright file="EventProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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
    public abstract class EventProcessor<TEvent> : Grain where TEvent : IEvent
    {
        private readonly IEventProcessingLogger logger = new EventProcessingLogger();
        private readonly ISmsService smsService = new SmsService();

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
        ///     Gets the SMS service.
        /// </summary>
        /// <value>The SMS service.</value>
        [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
        protected ISmsService SmsService
        {
            get { return this.smsService; }
        }

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public virtual async Task ProcessEventAsync(TEvent @event)
        {
            await this.ProcessingEventAsync(@event, async e =>
            {
                string topicName = e.GetType().Name.ToUnderScope();
                TopicClient client = TopicClient.CreateFromConnectionString(SiloClusterConfig.ServiceBusConnectiongString, topicName);
                await client.SendAsync(new BrokeredMessage(e.ToJson()));
            });
        }

        /// <summary>
        ///     Processing event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="processing">The processing.</param>
        /// <returns>Task.</returns>
        protected Task ProcessingEventAsync(TEvent @event, Func<TEvent, Task> processing)
        {
            Task.Factory.StartNew(async () =>
            {
                try
                {
                    await processing.Invoke(@event);
                }
                catch (Exception e)
                {
                    this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e);
                }
            }).Forget(e => this.ErrorLogger.LogError(@event.EventId, @event, e.Message, e));

            return TaskDone.Done;
        }
    }
}