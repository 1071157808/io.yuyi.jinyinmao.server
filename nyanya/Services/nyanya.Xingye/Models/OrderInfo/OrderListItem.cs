// FileInformation: nyanya/nyanya.Xingye/OrderListItem.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:29 PM

using Infrastructure.Lib.Extensions;
using nyanya.Xingye.Helper;
using Xingye.Domain.Orders.ReadModels;

namespace nyanya.Xingye.Models
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
    }

    internal static partial class OrderInfoExtensions
    {
        internal static OrderListItem ToOrderListItem(this OrderInfo order)
        {
            OrderListItem item = new OrderListItem
            {
                ExtraInterest = order.ExtraInterest,
                Interest = order.Interest,
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
                Yield = order.Yield
            };

            item.ShowingStatus = OrderResponseHelper.GetOrderShowingStatus(order);

            item.Message = order.IsPaid ? "支付成功。" : order.HasResult ? order.PaymentInfoTransDesc : "正在支付中。";
            item.UseableItemCount = 0;

            return item;
        }
    }
}