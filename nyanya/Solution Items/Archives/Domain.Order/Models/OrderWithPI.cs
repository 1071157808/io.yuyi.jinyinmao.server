using System;
using Domain.Order.Services.Interfaces;

namespace Domain.Order.Models
{
    public class OrderWithPI : IOrder
    {
        public virtual int Duration { get; set; }

        public virtual decimal? ExtraInterest { get; set; }

        public virtual int? ItemId { get; set; }

        public virtual int OrderId { get; set; }

        public virtual string OrderIdentifier { get; set; }

        public virtual decimal Principal { get; set; }

        public virtual DateTime SettleDay { get; set; }

        public virtual OrderStatus Status { get; set; }

        public virtual string UserGuid { get; set; }

        public virtual decimal Yield { get; set; }
    }
}