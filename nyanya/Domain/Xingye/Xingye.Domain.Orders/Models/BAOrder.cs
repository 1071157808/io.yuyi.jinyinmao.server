// FileInformation: nyanya/Xingye.Domain.Orders/BAOrder.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using Xingye.Commands.Orders;

namespace Xingye.Domain.Orders.Models
{
    public class BAOrder : Order
    {
        public BAOrder()
        {
        }

        public BAOrder(string orderIdentifier)
            : base(orderIdentifier)
        {
            this.OrderType = OrderType.BankAcceptance;
        }
    }
}