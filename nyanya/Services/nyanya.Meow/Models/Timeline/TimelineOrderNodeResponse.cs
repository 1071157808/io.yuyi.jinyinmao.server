// FileInformation: nyanya/nyanya.Meow/TimelineOrderNodeResponse.cs
// CreatedTime: 2014/09/17   10:41 AM
// LastUpdatedTime: 2014/09/17   10:52 AM

using Cat.Commands.Orders;
using Cat.Domain.Orders.ReadModels;
using Infrastructure.Lib.Extensions;
using nyanya.Meow.Helper;

namespace nyanya.Meow.Models
{
    /// <summary>
    /// </summary>
    public class TimelineOrderNodeResponse
    {
        /// <summary>
        ///     额外收益
        /// </summary>
        public decimal ExtraInterest { get; set; }

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
        public string OrderTime { get; set; }

        /// <summary>
        ///     订单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        ///     投资周期
        /// </summary>
        public int Period { get; set; }

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
        ///     项目的单价，即每份多少钱
        /// </summary>
        public decimal ProductUnitPrice { get; set; }

        /// <summary>
        ///     最迟还款日
        /// </summary>
        public string RepaymentDeadline { get; set; }

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
        ///     可用道具数
        /// </summary>
        public int UseableItemCount
        {
            get { return 0; }
        }

        /// <summary>
        ///     起息日期
        /// </summary>
        public string ValueDate { get; set; }

        /// <summary>
        ///     订单的项目收益率
        /// </summary>
        public decimal Yield { get; set; }
    }

    internal static partial class OrderInfoExtensions
    {
        internal static TimelineOrderNodeResponse ToTimelineOrderNodeResponse(this OrderInfo order)
        {
            TimelineOrderNodeResponse response = new TimelineOrderNodeResponse
            {
                ExtraInterest = order.ExtraInterest,
                Interest = order.Interest,
                OrderIdentifier = order.OrderIdentifier,
                OrderNo = order.OrderNo,
                OrderTime = order.OrderTime.ToMeowFormat(),
                OrderType = order.OrderType,
                Period = (order.SettleDate - order.ValueDate).Days,
                Principal = order.Principal,
                ProductIdentifier = order.ProductIdentifier,
                ProductName = order.ProductName,
                ProductNo = order.ProductNo,
                ProductNumber = order.ProductNumber,
                ProductUnitPrice = order.ProductUnitPrice,
                RepaymentDeadline = order.RepaymentDeadline.ToMeowFormat(),
                SettleDate = order.SettleDate.ToMeowFormat(),
                ShareCount = order.ShareCount,
                TotalAmount = order.TotalAmount,
                ValueDate = order.ValueDate.ToMeowFormat(),
                Yield = order.Yield,
            };

            response.ShowingStatus = OrderResponseHelper.GetOrderShowingStatus(order);

            return response;
        }
    }
}