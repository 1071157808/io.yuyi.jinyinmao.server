// FileInformation: nyanya/Infrastructure.Lib.CQRS/CQRSConfigration.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/07/02   3:39 PM

using System;
using Infrastructure.Lib.CQRS.MessageLogs;
using Infrastructure.Lib.Dependencies;

namespace Infrastructure.Lib.CQRS.Config
{
    public class CqrsConfigration : IDisposable
    {
        private static readonly IConfigSource configSource = new ConfigSource();

        public CqrsConfigration()
        {
            this.DependencyResolver = EmptyResolver.Instance;
            this.ServicesContainer = new DefaultServices(this);
        }

        public ICommandLogStore CommandLogStore
        {
            get { return this.ServicesContainer.GetService(typeof(ICommandLogStore)) as ICommandLogStore; }
        }

        public IDependencyResolver DependencyResolver { get; set; }

        public ServicesContainer ServicesContainer { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion IDisposable Members
    }
}