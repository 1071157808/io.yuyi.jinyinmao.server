// FileInformation: nyanya/nyanya.Meow/OrderResponseHelper.cs
// CreatedTime: 2014/08/29   2:39 PM
// LastUpdatedTime: 2014/09/01   5:28 PM

using Cat.Domain.Orders.ReadModels;
using nyanya.Meow.Models;
using System;

namespace nyanya.Meow.Helper
{
    internal static class OrderResponseHelper
    {
        internal static OrderShowingStatus GetOrderShowingStatus(OrderInfo order)
        {
            if (!order.HasResult)
            {
                return OrderShowingStatus.Paying;
            }
            if (order.IsPaid)
            {
                if (order.ValueDate.Date > DateTime.Now.Date)
                {
                    return OrderShowingStatus.WaitForInterest;
                }
                if (order.SettleDate.Date > DateTime.Now.Date)
                {
                    return OrderShowingStatus.Interesting;
                }
                return OrderShowingStatus.Interested;
            }
            return OrderShowingStatus.Failed;
        }
    }
}