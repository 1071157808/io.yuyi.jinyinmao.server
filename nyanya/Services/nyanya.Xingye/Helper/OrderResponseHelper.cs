// FileInformation: nyanya/nyanya.Xingye/OrderResponseHelper.cs
// CreatedTime: 2014/09/01   10:19 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using System;
using nyanya.Xingye.Models;
using Xingye.Domain.Orders.ReadModels;

namespace nyanya.Xingye.Helper
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