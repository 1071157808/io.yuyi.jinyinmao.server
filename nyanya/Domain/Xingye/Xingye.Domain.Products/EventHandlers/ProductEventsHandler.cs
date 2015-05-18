// FileInformation: nyanya/Xingye.Domain.Products/ProductEventsHandler.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System.Threading.Tasks;
using Domian.Config;
using Domian.Events;
using Xingye.Events.Products;

namespace Xingye.Domain.Products.EventHandlers
{
    public class ProductEventsHandler : EventHandlerBase,
        IEventHandler<BAProductUnShelved>,
        IEventHandler<BAProductLaunched>,
        IEventHandler<TAProductLaunched>
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
    }
}