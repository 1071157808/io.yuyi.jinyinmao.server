// FileInformation: nyanya/nyanya.Xingye/OrderInfoResponse.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   2:07 PM

using Infrastructure.Lib.Extensions;
using nyanya.Xingye.Helper;
using System;
using System.Linq;
using Xingye.Commands.Orders;
using Xingye.Domain.Orders.ReadModels;
using Xingye.Domain.Users.Helper;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     订单信息
    /// </summary>
    public class OrderInfoResponse
    {
        /// <summary>
        ///     银行卡开户地
        /// </summary>
        public string BankCardCity { get; set; }

        /// <summary>
        ///     开户行城市名
        /// </summary>
        public string BankCardCityName
        {
            get { return this.BankCardCity.Split(new[] { '|' }).FirstOrDefault(); }
        }

        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     开户行省份名
        /// </summary>
        public string BankCardProvinceName
        {
            get { return this.BankCardCity.Split(new[] { '|' }).LastOrDefault(); }
        }

        /// <summary>
        ///     银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        ///     委托协议名称
        /// </summary>
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        ///     质押物大图链接
        /// </summary>
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     质押物缩略图链接
        /// </summary>
        public string EndorseImageThumbnailLink { get; set; }

        /// <summary>
        ///     额外收益
        /// </summary>
        public decimal ExtraInterest { get; set; }

        /// <summary>
        ///     是否已经有支付结果
        /// </summary>
        public bool HasResult { get; set; }

        /// <summary>
        ///     收益
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        ///     投资者手机号
        /// </summary>
        public string InvestorCellphone { get; set; }

        /// <summary>
        ///     投资者证件类型
        /// </summary>
        public int InvestorCredential { get; set; }

        /// <summary>
        ///     投资者证件编号
        /// </summary>
        public string InvestorCredentialNo { get; set; }

        /// <summary>
        ///     投资者真实姓名
        /// </summary>
        public string InvestorRealName { get; set; }

        /// <summary>
        ///     是否支付成功
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        ///     提示信息
        /// </summary>
        public string Message
        {
            get { return this.IsPaid ? "支付成功。" : this.HasResult ? this.PaymentInfoTransDesc : "正在支付中。"; }
        }

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
        ///     Gets or sets the payment information trans desc.
        /// </summary>
        /// <value>
        ///     The payment information trans desc.
        /// </value>
        public string PaymentInfoTransDesc { get; set; }

        /// <summary>
        ///     投资周期
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        ///     质押借款协议名称
        /// </summary>
        public string PledgeAgreementName { get; set; }

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
        ///     Gets or sets the repayment deadline.
        /// </summary>
        /// <value>
        ///     The repayment deadline.
        /// </value>
        public string RepaymentDeadline { get; set; }

        /// <summary>
        ///     支付清算时间
        /// </summary>
        public string ResultTime { get; set; }

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
        ///     用户唯一标识
        /// </summary>
        public string UserIdentifier { get; set; }

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
        #region Internal Methods

        internal static OrderInfoResponse ToOrderInfoResponse(this OrderInfo order)
        {
            OrderInfoResponse orderInfoResponse = new OrderInfoResponse
            {
                BankCardCity = order.BankCardCity,
                BankCardNo = order.BankCardNo,
                BankName = order.BankName,
                ConsignmentAgreementName = order.ConsignmentAgreementName,
                EndorseImageLink = order.EndorseImageLink,
                EndorseImageThumbnailLink = order.EndorseImageThumbnailLink,
                ExtraInterest = order.ExtraInterest,
                HasResult = order.HasResult,
                Interest = order.Interest,
                InvestorCellphone = order.InvestorCellphone,
                InvestorCredential = order.InvestorCredential.CredentialTypeCode(),
                InvestorCredentialNo = order.InvestorCredentialNo.HideStringBalance(),
                InvestorRealName = order.InvestorRealName,
                IsPaid = order.IsPaid,
                OrderIdentifier = order.OrderIdentifier,
                OrderNo = order.OrderNo,
                OrderTime = order.OrderTime.ToMeowFormat(),
                OrderType = order.OrderType,
                Period = (order.SettleDate - order.ValueDate).Days,
                PledgeAgreementName = order.PledgeAgreementName,
                Principal = order.Principal,
                ProductIdentifier = order.ProductIdentifier,
                ProductName = order.ProductName,
                ProductNo = order.ProductNo,
                ProductNumber = order.ProductNumber,
                ProductUnitPrice = order.ProductUnitPrice,
                ResultTime = order.ResultTime.GetValueOrDefault(DateTime.Now).ToMeowFormat(),
                RepaymentDeadline = order.RepaymentDeadline.ToMeowFormat(),
                SettleDate = order.SettleDate.ToMeowFormat(),
                ShareCount = order.ShareCount,
                TotalAmount = order.TotalAmount,
                UserIdentifier = order.UserIdentifier,
                ValueDate = order.ValueDate.ToMeowFormat(),
                Yield = order.Yield,
                PaymentInfoTransDesc = order.PaymentInfoTransDesc
            };

            orderInfoResponse.ShowingStatus = OrderResponseHelper.GetOrderShowingStatus(order);

            return orderInfoResponse;
        }

        #endregion Internal Methods
    }
}