// FileInformation: nyanya/nyanya.Cat/OrderResponseHelper.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   4:31 PM

using Cat.Domain.Orders.ReadModels;
using nyanya.Cat.Models;
using System;

namespace nyanya.Cat.Helper
{
    internal static class OrderResponseHelper
    {
        #region Private Methods

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
        internal static OrderShowingStatus GetOrderShowingStatus(Xingye.Domain.Orders.ReadModels.OrderInfo order)
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

        #endregion Private Methods
    }
}