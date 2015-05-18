
using System.Collections.Generic;
using Cat.Commands.Orders;

namespace Cat.Domain.Orders.Models
{
    public partial class ZCBOrder : Order
    {
        public ZCBOrder()
        {
        }

        public ZCBOrder(string orderIdentifier)
            : base(orderIdentifier)
        {
            this.OrderType = OrderType.ZCBAcceptance;
        }
    }
}
