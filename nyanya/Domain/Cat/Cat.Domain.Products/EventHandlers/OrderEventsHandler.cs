// FileInformation: nyanya/Cat.Domain.Products/OrderEventsHandler.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using Domian.Config;
using Domian.Events;

namespace Cat.Domain.Products.EventHandlers
{
    public class OrderEventsHandler : EventHandlerBase //, IEventHandler<OrderPaymentSuccessed>//, IEventHandler<OrderPaymentFailed>
    {
        #region Public Constructors

        public OrderEventsHandler(CqrsConfiguration config)
            : base(config)
        {
        }

        #endregion Public Constructors

        //public async Task Handler(OrderPaymentFailed @event)
        //{
        //    await this.DoAsync(e =>
        //    {
        //        Product product = new Product(e.ProductIdentifier);
        //        product.UnfreezeShareCount(e.ShareCount);
        //        return null;
        //    }, @event);
        //}

        //public async Task Handler(OrderPaymentSuccessed @event)
        //{
        //    await this.DoAsync(async e =>
        //    {
        //        Product product = new Product(e.ProductIdentifier);
        //        await product.Paid(e.ShareCount);
        //    }, @event);
        //}
    }
}