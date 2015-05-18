// FileInformation: nyanya/Domain.Order/OrderSummary.cs
// CreatedTime: 2014/04/01   2:47 PM
// LastUpdatedTime: 2014/04/21   11:51 PM

using System;

namespace Domain.Order.Models
{
    public class OrderSummary
    {
        public virtual DateTime CreatedAt { get; set; }

        public virtual Nullable<decimal> ExpectedPrice { get; set; }

        public virtual Nullable<decimal> ExpectedYield { get; set; }

        public virtual int OrderId { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual Nullable<decimal> Price { get; set; }

        public virtual string ProductIdentifer { get; set; }

        public virtual string ProductInfo { get; set; }

        public virtual string UserGuid { get; set; }

        public virtual int UserId { get; set; }
    }
}