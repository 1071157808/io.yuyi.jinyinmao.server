// FileInformation: nyanya/Cat.Domain.Products/ProductEventsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using System.Threading.Tasks;
using Cat.Domain.Products.Services;
using Cat.Events.Products;
using Domian.Config;
using Domian.Events;

namespace Cat.Domain.Products.EventHandlers
{
    public class ProductEventsHandler : EventHandlerBase,
        IEventHandler<BAProductUnShelved>,
        IEventHandler<BAProductLaunched>,
        IEventHandler<TAProductLaunched>,
        IEventHandler<ZCBProductLaunched>,
        IEventHandler<ZCBUpdateShareCounted>
    {
        public ProductEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region IEventHandler<BAProductLaunched> Members

        public Task Handler(BAProductLaunched @event)
        {
            return null;
        }

        #endregion IEventHandler<BAProductLaunched> Members

        #region IEventHandler<BAProductUnShelved> Members

        public Task Handler(BAProductUnShelved @event)
        {
            return null;
        }

        #endregion IEventHandler<BAProductUnShelved> Members

        #region IEventHandler<TAProductLaunched> Members

        public Task Handler(TAProductLaunched @event)
        {
            return null;
        }

        #endregion IEventHandler<TAProductLaunched> Members

        #region IEventHandler<ZCBProductLaunched> Members

        public Task Handler(ZCBProductLaunched @event)
        {
            return null;
        }
        #endregion IEventHandler<ZCBProductLaunched> Members

        #region IEventHandler<ZCBUpdateShareCounted> Members

        public Task Handler(ZCBUpdateShareCounted @event)
        {
            return null;
        }

        #endregion IEventHandler<ZCBUpdateShareCounted> Members
    }
}