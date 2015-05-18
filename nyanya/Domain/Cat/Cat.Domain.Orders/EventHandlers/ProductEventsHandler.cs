// FileInformation: nyanya/Cat.Domain.Orders/ProductEventsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:48 PM

using System.Threading.Tasks;
using Cat.Domain.Orders.Services;
using Cat.Events.Products;
using Domian.Config;
using Domian.Events;

namespace Cat.Domain.Orders.EventHandlers
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