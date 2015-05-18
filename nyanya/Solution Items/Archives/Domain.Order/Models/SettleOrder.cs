// FileInformation: nyanya/Domain.Order/SettleOrder.cs
// CreatedTime: 2014/04/30   4:51 PM
// LastUpdatedTime: 2014/05/08   1:15 PM

using System;

namespace Domain.Order.Models
{
    public class SettleOrder
    {
        public virtual int Id { get; set; }

        public virtual int OrderId { get; set; }

        public virtual DateTime SettleTime { get; set; }

        public virtual string UserGuid { get; set; }

        public virtual int UserId { get; set; }
    }
}