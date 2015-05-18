// FileInformation: nyanya/Xingye.Domain.Orders/TAOrder.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   5:23 PM

using Xingye.Commands.Orders;

namespace Xingye.Domain.Orders.Models
{
    public class TAOrder : Order
    {
        public TAOrder()
        {
        }

        public TAOrder(string orderIdentifier)
            : base(orderIdentifier)
        {
            this.OrderType = OrderType.TradeAcceptance;
        }
    }
}