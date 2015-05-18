// FileInformation: nyanya/Domian/CqrsConfiguration.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/11   12:01 PM

using System;
using Domian.Bus;
using Domian.Commands;
using Domian.Events;
using Domian.Logs;
using Infrastructure.Lib;
using Infrastructure.Lib.Dependencies;
using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.SMS;

namespace Domian.Config
{
    public class CqrsConfiguration : IDisposable
    {
        public CqrsConfiguration()
        {
            this.DependencyResolver = EmptyResolver.Instance;
            this.Services = new DefaultServicesContainer(this);
        }

        public ICommandBus CommandBus
        {
            get { return this.Services.GetService(typeof(ICommandBus)) as ICommandBus; }
        }

        public ILogger CommandBusLogger
        {
            get
            {
                LogsConfig config = this.Services.GetService(typeof(LogsConfig)) as LogsConfig;
                return config != null ? config.CommandBusLogger : new DefaultLogger();
            }
        }

        public ILogger CommandHandlerLogger
        {
            get
            {
                LogsConfig config = this.Services.GetService(typeof(LogsConfig)) as LogsConfig;
                return config != null ? config.CommandHandlerLogger : new DefaultLogger();
            }
        }

        public ICommandHandlers CommandHandlers
        {
            get { return this.Services.GetService(typeof(ICommandHandlers)) as ICommandHandlers; }
        }

        public ICommandLogStore CommandLogStore
        {
            get { return this.Services.GetService(typeof(ICommandLogStore)) as ICommandLogStore; }
        }

        public ILogger CommandStoreLogger
        {
            get
            {
                LogsConfig config = this.Services.GetService(typeof(LogsConfig)) as LogsConfig;
                return config != null ? config.CommandStoreLogger : new DefaultLogger();
            }
        }

        public IDependencyResolver DependencyResolver { get; set; }

        public IEventBus EventBus
        {
            get { return this.Services.GetService(typeof(IEventBus)) as IEventBus; }
        }

        public ILogger EventBusLogger
        {
            get
            {
                LogsConfig config = this.Services.GetService(typeof(LogsConfig)) as LogsConfig;
                return config != null ? config.EventBusLogger : new DefaultLogger();
            }
        }

        public ILogger EventDispatcherLogger
        {
            get
            {
                LogsConfig config = this.Services.GetService(typeof(LogsConfig)) as LogsConfig;
                return config != null ? config.EventDispatcherLogger : new DefaultLogger();
            }
        }

        public ILogger EventHandlerLogger
        {
            get
            {
                LogsConfig config = this.Services.GetService(typeof(LogsConfig)) as LogsConfig;
                return config != null ? config.EventHandlerLogger : new DefaultLogger();
            }
        }

        public IEventHandlers EventHandlers
        {
            get { return this.Services.GetService(typeof(IEventHandlers)) as IEventHandlers; }
        }

        public LogsConfig Logs
        {
            get { return this.Services.GetService(typeof(LogsConfig)) as LogsConfig; }
        }

        public ServicesContainer Services { get; set; }

        public ISmsAlertsService SmsAlertsService
        {
            get { return this.Services.GetService(typeof(ISmsAlertsService)) as ISmsAlertsService; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Services.Dispose();
        }

        #endregion IDisposable Members
    }
}