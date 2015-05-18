// FileInformation: nyanya/Xingye.Domain.Orders/ProductEventsHandler.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System.Threading.Tasks;
using Domian.Config;
using Domian.Events;
using Xingye.Domain.Orders.Services;
using Xingye.Events.Products;

namespace Xingye.Domain.Orders.EventHandlers
{
    public class ProductEventsHandler : EventHandlerBase,
        IEventHandler<ProductRepaid>
    {
        public ProductEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #region IEventHandler<ProductRepaid> Members

        public async Task Handler(ProductRepaid @event)
        {
            await this.DoAsync(async e => { await new OrderService().SetOrdersRepaidForProductAsync(e.ProductIdentifier); }, @event);
        }

        #endregion IEventHandler<ProductRepaid> Members
    }
}