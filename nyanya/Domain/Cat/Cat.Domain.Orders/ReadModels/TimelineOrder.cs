// FileInformation: nyanya/Cat.Domain.Orders/TimelineOrder.cs
// CreatedTime: 2014/09/15   12:13 PM
// LastUpdatedTime: 2014/09/15   5:30 PM

using System;
using Cat.Commands.Orders;
using Domian.Models;

namespace Cat.Domain.Orders.Models
{
    public class TimelineOrder : IReadModel
    {
        public decimal Interest { get; set; }

        public string OrderIdentifier { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderType OrderType
        {
            get
            {
                if (this.TypeCode == 10)
                {
                    return OrderType.BankAcceptance;
                }

                if (this.TypeCode == 20)
                {
                    return OrderType.TradeAcceptance;
                }

                return OrderType.BankAcceptance;
            }
        }

        public decimal Principal { get; set; }

        public string ProductIdentifier { get; set; }

        public DateTime RepaymentDeadline { get; set; }

        public int TypeCode { get; set; }

        public string UserIdentifier { get; set; }
    }
}