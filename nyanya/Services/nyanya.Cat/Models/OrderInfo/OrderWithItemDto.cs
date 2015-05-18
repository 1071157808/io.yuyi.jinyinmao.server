// FileInformation: nyanya/nyanya.Cat/OrderWithItemDto.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   10:53 AM

using System;

namespace nyanya.Cat.Order
{
    /// <summary>
    ///     OrderWithItemDto
    /// </summary>
    public class OrderWithItemDto
    {
        #region Public Properties

        /// <summary>
        ///     下单时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     预期收益
        /// </summary>
        public decimal ExpectedPrice { get; set; }

        /// <summary>
        ///     道具增收
        /// </summary>
        public decimal ExtraInterest { get; set; }

        /// <summary>
        ///     道具名称
        /// </summary>
        public string ItemTitle { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>
        ///     The order identifier.
        /// </value>
        public int OrderId { get; set; }

        /// <summary>
        ///     订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        ///     购买额度
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        ///     可以使用道具的数量
        /// </summary>
        public int UseableItemCount { get; set; }

        #endregion Public Properties
    }

    //internal static class OrderSummaryExtensions
    //{
    //    internal static OrderWithItemDto ToOrderWithItemDto(this OrderSummary order, OHPItem item)
    //    {
    //        OrderWithItemDto oi = new OrderWithItemDto
    //        {
    //            CreatedAt = order.CreatedAt,
    //            ExpectedPrice = order.ExpectedPrice.GetValueOrDefault(),
    //            ExtraInterest = 0,
    //            ItemTitle = "",
    //            OrderId = order.OrderId,
    //            OrderNo = order.OrderNo,
    //            Price = order.Price.GetValueOrDefault(),
    //            UseableItemCount = 0
    //        };

    //        if (order.OrderStatus == OrderStatus.Paid)
    //        {
    //            oi.UseableItemCount = 1;
    //        }

    //        if (item != null)
    //        {
    //            oi.UseableItemCount = -1;
    //            oi.ItemTitle = item.Category.CategoryTitle;
    //            oi.ExtraInterest = item.ExtraInterest.GetValueOrDefault();
    //        }

    //        return oi;
    //    }
    //}
}