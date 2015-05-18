// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-03-04  6:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-13  6:24 PM
// ***********************************************************************
// <copyright file="EventBus.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Domian.Config;
using Domian.Events;
using Infrastructure.Lib;
using Infrastructure.Lib.Disposal;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;

namespace Domian.Bus.Implementation
{
    public class EventBus : DisposableObject, IEventBus
    {
        private static readonly RabbitMqServer mqServer = new RabbitMqServer(ConfigurationManager.AppSettings.Get("EventProcessorAddress"));
        private readonly CqrsConfiguration config;
        private readonly Lazy<IStoreEvents> eventStore;
        private readonly Random r = new Random();

        public EventBus(CqrsConfiguration config)
        {
            this.eventStore = new Lazy<IStoreEvents>(this.InitEventStore);
            this.config = config;
        }

        private ISmsAlertsService AlertService
        {
            get { return this.config.SmsAlertsService; }
        }

        private IStoreEvents EventStore
        {
            get { return this.eventStore.Value; }
        }

        private ILogger Logger
        {
            get { return this.config.EventBusLogger; }
        }

        #region IEventBus Members

        public void Publish<T>(T @event) where T : IEvent
        {
            Task.Run(() => this.DoPublish(@event));
        }

        public void Publish<T>(IEnumerable<T> events) where T : IEvent
        {
            foreach (T @event in events)
            {
                T e = @event;
                Task.Run(() => this.DoPublish(e));
            }
        }

        #endregion IEventBus Members

        /// <summary>
        ///     Disposes the object.
        /// </summary>
        /// <param name="disposing">
        ///     A <see cref="System.Boolean" /> value which indicates whether
        ///     the object should be disposed explicitly.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.EventStore.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoPublish<T>(T @event, int retryCount = 0) where T : IEvent
        {
            try
            {
                using (IMessageProducer mqClient = mqServer.CreateMessageProducer())
                {
                    mqClient.Publish(@event);
                }
            }
            catch (Exception e)
            {
                this.Logger.Error(e, NyanyaResources.EventDispatcher_UnexpectedException.FmtWith(e.GetType(), @event.EventId, e.Message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.Logger.Info("Error => {0}".FmtWith(@event.EventId));
            }

            //            try
            //            {
            //                this.Logger.Info("Begin => {0}|{1}|{2}".FmtWith(@event.GetType(), retryCount, @event.EventId));
            //
            //                using (IEventStream stream = this.EventStore.OpenStream(@event.SourceId, 0))
            //                {
            //                    Dictionary<string, object> headers = new Dictionary<string, object>
            //                    {
            //                        { "EventId", @event.EventId }, { "EventSourceId", @event.SourceId }, { "EventType", @event.SourceType }
            //                    };
            //
            //                    stream.Add(new EventMessage
            //                    {
            //                        Body = @event,
            //                        Headers = headers
            //                    });
            //
            //                    stream.CommitChanges(@event.EventId);
            //
            //                    if (@event.SourceType != null && typeof(IHasMemento).IsAssignableFrom(@event.SourceType))
            //                    {
            //                        object source = Activator.CreateInstance(@event.SourceType, @event.SourceId);
            //                        string snapshot = ((IHasMemento)source).GetMemento().Value;
            //                        this.EventStore.Advanced.AddSnapshot(new Snapshot(stream.StreamId, stream.StreamRevision, snapshot));
            //                    }
            //                }
            //
            //                this.Logger.Info("End => {0}|{1}|{2}".FmtWith(@event.GetType(), retryCount, @event.EventId));
            //            }
            //            catch (ConcurrencyException e)
            //            {
            //                this.Logger.Info("Error => {0}|{1}|{2}".FmtWith(@event.GetType(), retryCount, @event.EventId));
            //                if (retryCount < 5)
            //                {
            //                    this.DoPublish(@event, ++retryCount);
            //                    Thread.Sleep(new TimeSpan(this.r.Next(0, 100)));
            //                }
            //                this.Logger.Error(e, NyanyaResources.EventBus_NEventStore_ConcurrencyException.FmtWith(e.GetType(), @event.EventId, @event.ToJson(), e.Message));
            //                // ReSharper disable once CSharpWarnings::CS4014
            //                this.AlertService.AlertAsync(NyanyaResources.Alert_EventBus_NEventStore_ConcurrencyException.FmtWith(e.GetType(), @event.EventId, @event.ToJson(), e.Message));
            //            }
            //            catch (Exception e)
            //            {
            //                this.Logger.Error(e, NyanyaResources.EventBus_UnexpectedException.FmtWith(e.GetType(), @event.EventId, @event.ToJson(), e.Message));
            //                // ReSharper disable once CSharpWarnings::CS4014
            //                this.AlertService.AlertAsync(NyanyaResources.Alert_EventBus_UnexpectedException.FmtWith(e.GetType(), @event.EventId, @event.ToJson(), e.Message));
            //                this.Logger.Info("Error => {0}|{1}|{2}".FmtWith(@event.GetType(), retryCount, @event.EventId));
            //            }
        }

        private IStoreEvents InitEventStore()
        {
            return Wireup.Init()
                .UsingSqlPersistence("EventStore")
                .WithDialect(new MsSqlDialect())
                .WithStreamIdHasher(arg => arg ?? "Anonymous")
                .InitializeStorageEngine()
                .UsingJsonSerialization()
                .UsingAsynchronousDispatchScheduler()
                .DispatchTo(new EventDispatcher(this.config))
                .Build();
        }
    }
}