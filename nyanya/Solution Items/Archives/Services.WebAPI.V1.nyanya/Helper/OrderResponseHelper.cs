// FileInformation: nyanya/Services.WebAPI.V1.nyanya/OrderResponseHelper.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/25   3:20 PM

using System;
using Cqrs.Domain.Orders.ReadModels;
using Services.WebAPI.V1.nyanya.Models;

namespace Services.WebAPI.V1.nyanya.Helper
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

        #endregion Private Methods
    }
}