// FileInformation: nyanya/Domain.Order/OrderListItem.cs
// CreatedTime: 2014/04/22   6:56 PM
// LastUpdatedTime: 2014/04/23   3:33 PM

using System;

namespace Domain.Order.Models
{
    public class OrderListItem
    {
        public virtual Nullable<decimal> ExtraInterest { get; set; }

        public virtual int Id { get; set; }

        public virtual decimal Interest { get; set; }

        public virtual Nullable<int> ItemId { get; set; }

        public virtual string OrderIdentifier { get; set; }

        public virtual DateTime OrderTime { get; set; }

        public virtual decimal Principal { get; set; }

        public virtual DateTime SettleDay { get; set; }

        public virtual OrderStatus Status { get; set; }

        public virtual string UserGuid { get; set; }

        public virtual int UserId { get; set; }
    }
}