
using System;
using System.ComponentModel.DataAnnotations;
using Cat.Commands.Products;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Internal.Models
{
    /// <summary>
    /// 资产包产品上架请求
    /// </summary>
    public class ZCBHitShelvesRequest : IRequestModel
    {
        /// <summary>
        ///     票据大图链接
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     票据缩略图链接
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string EndorseImageThumbnailLink { get; set; }

        /// <summary>
        ///     售卖结束时间
        /// </summary>
        [Required]
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     委托协议
        /// </summary>
        [Required]
        [MaxLength(20000)]
        public string ConsignmentAgreement { get; set; }

        /// <summary>
        ///     融资总份数
        /// </summary>
        [Required]
        [Range(1, 2000000000)]
        public int FinancingSumCount { get; set; }

        /// <summary>
        ///     单笔订单最大购买份数
        /// </summary>
        [Required]
        [Range(1, 2000000000)]
        public int MaxShareCount { get; set; }

        /// <summary>
        ///     单笔订单最小购买份数
        /// </summary>
        [Required]
        [Range(1, 2000000000)]
        public int MinShareCount { get; set; }

        /// <summary>
        ///     项目周期
        /// </summary>
        [Required]
        [Range(1, 36500)]
        public int Period { get; set; }

        /// <summary>
        ///     提前购买结束时间
        /// </summary>
        public DateTime? PreEndSellTime { get; set; }

        /// <summary>
        ///     提前购买开始时间
        /// </summary>
        public DateTime? PreStartSellTime { get; set; }

        /// <summary>
        ///     担保协议
        /// </summary>
        [Required]
        public string PledgeAgreement { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        ///     产品编号，资产包产品以ZCB开头
        /// </summary>
        [Required]
        public string ProductNo { get; set; }

        /// <summary>
        ///     项目子编号
        /// </summary>
        [Required]
        public string SubProductNo { get; set; }

        /// <summary>
        ///     产品期数
        /// </summary>
        [Required]
        [Range(0, 1000000000)]
        public int ProductNumber { get; set; }

        /// <summary>
        ///     最迟还款日期
        /// </summary>
        [Required]
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        [Required]
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     每一份的单价
        /// </summary>
        [Range(1, 2000000000)]
        public int UnitPrice { get; set; }

        /// <summary>
        ///     固定起息日期
        /// </summary>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     起息方式 10=>购买当天起息，20=>指定日期起息
        /// </summary>
        [AvailableValues(ValueDateMode.FixedDate, ValueDateMode.T0, ValueDateMode.T1)]
        public ValueDateMode ValueDateMode { get; set; }

        /// <summary>
        ///     收益率（保留位数为最小有效位数）
        /// </summary>
        [Required]
        [Range(0, 100)]
        public decimal Yield { get; set; }

        /// <summary>
        ///     结息日
        /// </summary>
        [Required]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     授权委托书名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ConsignmentAgreementName { get; set; }

        /// <summary>
        ///     投资协议名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string PledgeAgreementName { get; set; }

        /// <summary>
        ///     产品下次可取现额度
        /// </summary>
        [Range(1, 2000000000)]
        public decimal PerRemainRedeemAmount { get; set; }
    }
    internal static partial class HitShelvesRequestExtensions
    {
        internal static LaunchZCBProduct ToLaunchZCBProduct(this ZCBHitShelvesRequest request)
        {
            return new LaunchZCBProduct
            {
                EnableSale = 1,  //缺省情况下，资产包产品都是允许开卖
                ConsignmentAgreementName = request.ConsignmentAgreementName,
                ConsignmentAgreement = request.ConsignmentAgreement,
                EndorseImageLink = request.EndorseImageLink,
                EndorseImageThumbnailLink = request.EndorseImageThumbnailLink,
                EndSellTime = request.EndSellTime,
                FinancingSumCount = request.FinancingSumCount,
                MaxShareCount = request.MaxShareCount,
                MinShareCount = request.MinShareCount,
                Period = request.Period,
                PledgeAgreementName = request.PledgeAgreementName,
                PledgeAgreement = request.PledgeAgreement,
                PreEndSellTime = request.PreEndSellTime,
                PreStartSellTime = request.PreStartSellTime,
                ProductName = request.ProductName,
                ProductNo = request.ProductNo,
                SubProductNo = request.SubProductNo,
                ProductNumber = request.ProductNumber,
                RepaymentDeadline = request.RepaymentDeadline,
                SettleDate = request.SettleDate,
                StartSellTime = request.StartSellTime,
                UnitPrice = request.UnitPrice,
                ValueDate = request.ValueDate,
                ValueDateMode = request.ValueDateMode,
                Yield = request.Yield,
                ProductCategory = ProductCategory.JINYINMAO,
                PerRemainRedeemAmount = request.PerRemainRedeemAmount
            };
        }
    }
}
