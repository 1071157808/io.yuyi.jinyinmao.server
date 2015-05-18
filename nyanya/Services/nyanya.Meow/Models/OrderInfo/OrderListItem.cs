// FileInformation: nyanya/nyanya.Meow/OrderListItem.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:28 PM

using System;
using Cat.Domain.Orders.ReadModels;
using Infrastructure.Lib.Extensions;
using nyanya.Meow.Helper;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     OrderListItem
    /// </summary>
    public class OrderListItem
    {
        /// <summary>
        ///     额外收益
        /// </summary>
        public decimal ExtraInterest { get; set; }

        /// <summary>
        ///     预期收益
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        ///     提示信息
        /// </summary>
        public string Message { get; set; }

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
        public string OrderTime { get; set; }

        /// <summary>
        ///     订单本金
        /// </summary>
        public decimal Principal { get; set; }

        /// <summary>
        ///     项目唯一标识
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     项目名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     项目编号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        ///     项目期数
        /// </summary>
        public int ProductNumber { get; set; }

        /// <summary>
        ///     结息日期
        /// </summary>
        public string SettleDate { get; set; }

        /// <summary>
        ///     订单的购买份数
        /// </summary>
        public int ShareCount { get; set; }

        /// <summary>
        ///     订单的显示状态
        /// </summary>
        public OrderShowingStatus ShowingStatus { get; set; }

        /// <summary>
        ///     订单的本息总额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        ///     可用道具数量
        /// </summary>
        public int UseableItemCount { get; set; }

        /// <summary>
        ///     起息日期
        /// </summary>
        public string ValueDate { get; set; }

        /// <summary>
        ///     订单的预期收益率
        /// </summary>
        public decimal Yield { get; set; }

        /// <summary>
        ///     是否已操作还款
        /// </summary>
        public bool IsRepaid { get; set; }

        /// <summary>
        ///     Gets or sets the server time.
        /// </summary>
        /// <value>
        ///     The server time.
        /// </value>
        public string ServerTime { get; set; }

        /// <summary>
        ///     Gets or sets the repayment deadline.
        /// </summary>
        /// <value>
        ///     The repayment deadline.
        /// </value>
        public string RepaymentDeadline { get; set; }
    }

    internal static partial class OrderInfoExtensions
    {
        internal static OrderListItem ToOrderListItem(this OrderInfo order)
        {
            OrderListItem item = new OrderListItem
            {
                ExtraInterest = order.ExtraInterest.ToFloor(2),
                Interest = order.Interest.ToFloor(2),
                OrderIdentifier = order.OrderIdentifier,
                OrderNo = order.OrderNo,
                OrderTime = order.OrderTime.ToMeowFormat(),
                Principal = order.Principal,
                ProductIdentifier = order.ProductIdentifier,
                ProductName = order.ProductName,
                ProductNo = order.ProductNo,
                ProductNumber = order.ProductNumber,
                SettleDate = order.SettleDate.ToMeowFormat(),
                ShareCount = order.ShareCount,
                TotalAmount = order.TotalAmount,
                ValueDate = order.ValueDate.ToMeowFormat(),
                Yield = order.Yield,
                IsRepaid = order.IsRepaid,
                ServerTime = DateTime.Now.AddSeconds(2).ToMeowFormat(),
                RepaymentDeadline = order.RepaymentDeadline.ToMeowFormat()
            };

            item.ShowingStatus = OrderResponseHelper.GetOrderShowingStatus(order);

            item.Message = order.IsPaid ? "支付成功。" : order.HasResult ? order.PaymentInfoTransDesc : "正在支付中。";
            item.UseableItemCount = 0;

            return item;
        }
    }
}
