// FileInformation: nyanya/Domain.Order/TimelineItem.cs
// CreatedTime: 2014/04/05   2:54 AM
// LastUpdatedTime: 2014/04/26   11:01 AM

using System;

namespace Domain.Order.Models
{
    public enum TimeLineItemType
    {
        Beginning = 0,
        Order = 10,
        Today = 20,
        Product = 30,
        End = 40
    }

    public partial class TimelineItem
    {
        public virtual string Identifier { get; set; }

        public virtual Nullable<decimal> Interest { get; set; }

        public virtual Nullable<decimal> Principal { get; set; }

        public virtual int Status { get; set; }

        public virtual Nullable<DateTime> Time { get; set; }

        public virtual TimeLineItemType Type { get; set; }

        public virtual string UserGuid { get; set; }
    }
}