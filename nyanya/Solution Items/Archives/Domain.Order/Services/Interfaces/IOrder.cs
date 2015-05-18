using System;
using Domain.Order.Models;

namespace Domain.Order.Services.Interfaces
{
    public interface IOrder
    {
        int Duration { get; set; }

        int OrderId { get; set; }

        DateTime SettleDay { get; set; }

        OrderStatus Status { get; set; }

        decimal Yield { get; set; }
    }
}