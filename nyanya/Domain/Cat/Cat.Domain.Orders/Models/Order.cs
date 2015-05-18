// FileInformation: nyanya/Cqrs.Domain.Order/Order.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/16   7:38 PM

using Cat.Commands.Orders;
using Domian.Models;
using System;
using System.Collections.Generic;

namespace Cat.Domain.Orders.Models
{
    public partial class Order : IAggregateRoot
    {
        public Order(string orderIdentifier)
        {
            this.OrderIdentifier = orderIdentifier;
        }

        protected Order()
        {
        }

        public AgreementsInfo AgreementsInfo { get; set; }

        public long ClientType { get; set; }

        public decimal ExtraInterest { get; set; }

        public long FlgMoreI1 { get; set; }

        public long FlgMoreI2 { get; set; }

        public string FlgMoreS1 { get; set; }

        public string FlgMoreS2 { get; set; }

        public decimal Interest { get; set; }

        public InvestorInfo InvestorInfo { get; set; }

        public string IpClient { get; set; }

        public string OrderIdentifier { get; set; }

        public string OrderNo { get; set; }

        public DateTime OrderTime { get; set; }

        public OrderType OrderType { get; set; }

        public PaymentInfo PaymentInfo { get; set; }

        public decimal Principal { get; set; }

        public ProductInfo ProductInfo { get; set; }

        public ProductSnapshot ProductSnapshot { get; set; }

        public DateTime SettleDate { get; set; }

        public int ShareCount { get; set; }

        public string UserIdentifier { get; set; }

        public DateTime ValueDate { get; set; }

        public decimal Yield { get; set; }
    }
}
