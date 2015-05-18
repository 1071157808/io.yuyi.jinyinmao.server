// FileInformation: nyanya/nyanya.Meow/TimelineRefundNodeResponse.cs
// CreatedTime: 2014/09/16   6:18 PM
// LastUpdatedTime: 2014/09/17   11:17 AM

using System;
using System.Collections.Generic;
using Cat.Domain.Orders.ReadModels;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     RefundInfoOrderDto
    /// </summary>
    public class RefundInfoOrderDto
    {
        /// <summary>
        ///     收益
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        ///     订单唯一标识
        /// </summary>
        public string OrderIdentifier { get; set; }

        /// <summary>
        ///     订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        ///     下单时间
        /// </summary>
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     订单本金
        /// </summary>
        public decimal Principal { get; set; }
    }

    /// <summary>
    ///     TimelineRefundNodeResponse
    /// </summary>
    public class TimelineRefundNodeResponse
    {
        /// <summary>
        ///     Gets or sets the orders.
        /// </summary>
        /// <value>
        ///     The orders.
        /// </value>
        public IEnumerable<RefundInfoOrderDto> Orders { get; set; }

        /// <summary>
        ///     Gets or sets the product.
        /// </summary>
        /// <value>
        ///     The product.
        /// </value>
        public ProductInfoResponse Product { get; set; }
    }

    internal static class OrderExtensions
    {
        internal static RefundInfoOrderDto ToRefundInfoOrderDto(this OrderInfo order)
        {
            return new RefundInfoOrderDto
            {
                Interest = order.Interest + order.ExtraInterest,
                OrderIdentifier = order.OrderIdentifier,
                OrderNo = order.OrderNo,
                OrderTime = order.OrderTime,
                Principal = order.Principal
            };
        }
    }
}