// FileInformation: nyanya/Domian/EventDispatcher.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/09/01   12:26 PM

using System;
using System.Configuration;
using Domian.Config;
using Infrastructure.Lib;
using Infrastructure.Lib.Disposal;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.SMS;
using NEventStore;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;

namespace Domian.Bus.Implementation
{
    public class EventDispatcher : DisposableObject, IEventDispatcher
    {
        private static readonly RabbitMqServer mqServer;
        private readonly CqrsConfiguration config;

        static EventDispatcher()
        {
            mqServer = new RabbitMqServer(ConfigurationManager.AppSettings.Get("EventProcessorAddress"));
            mqServer.AutoReconnect = true;
            mqServer.DisablePriorityQueues = true;
        }

        public EventDispatcher(CqrsConfiguration config)
        {
            this.config = config;
        }

        private ISmsAlertsService AlertService
        {
            get { return this.config.SmsAlertsService; }
        }

        private ILogger Logger
        {
            get { return this.config.EventDispatcherLogger; }
        }

        #region IEventDispatcher Members

        public void Dispatch(ICommit commit)
        {
            try
            {
                this.Logger.Info("Begin => {0}".FmtWith(commit.CommitId));
                using (IMessageProducer mqClient = mqServer.CreateMessageProducer())
                {
                    foreach (EventMessage @event in commit.Events)
                    {
                        mqClient.Publish(@event.Body);
                    }
                }
                this.Logger.Info("Begin => {0}".FmtWith(commit.CommitId));
            }
            catch (Exception e)
            {
                this.Logger.Error(e, NyanyaResources.EventDispatcher_UnexpectedException.FmtWith(e.GetType(), commit.CommitId, e.Message));
                // ReSharper disable once CSharpWarnings::CS4014
                this.AlertService.AlertAsync(NyanyaResources.Alert_EventDispatcher_UnexpectedException.FmtWith(e.GetType(), commit.CommitId, e.Message));
                this.Logger.Info("Error => {0}".FmtWith(commit.CommitId));
            }
        }

        #endregion IEventDispatcher Members
    }
}