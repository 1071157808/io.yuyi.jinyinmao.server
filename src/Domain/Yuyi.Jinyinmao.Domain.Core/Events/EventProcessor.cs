// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:19 PM
// ***********************************************************************
// <copyright file="EventProcessor.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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
        /// <summary>
        ///     Gets the error logger.
        /// </summary>
        /// <value>The error logger.</value>
        public IEventProcessingLogger ErrorLogger { get; } = new EventProcessingLogger();

        /// <summary>
        ///     Gets the SMS service.
        /// </summary>
        /// <value>The SMS service.</value>
        protected ISmsService SmsService { get; } = new SmsService();

        /// <summary>
        ///     Processes the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <returns>Task.</returns>
        public virtual async Task ProcessEventAsync(TEvent @event) => await this.ProcessingEventAsync(@event, async e =>
        {
            string topicName = e.GetType().Name.ToUnderScope();
            TopicClient client = TopicClient.CreateFromConnectionString(SiloClusterConfig.ServiceBusConnectiongString, topicName);
            await client.SendAsync(new BrokeredMessage(e.ToJson()));
        });

        /// <summary>
        ///     Processing event asynchronous.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="processing">The processing.</param>
        /// <returns>Task.</returns>
        protected Task ProcessingEventAsync(TEvent @event, Func<TEvent, Task> processing)
        {
            Task.Factory.StartNew(() => processing.Invoke(@event).Forget(e => this.ErrorLogger.LogError(@event.EventId, e.Message, e)));

            return TaskDone.Done;
        }
    }
}