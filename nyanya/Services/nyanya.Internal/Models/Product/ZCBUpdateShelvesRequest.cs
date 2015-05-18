
using System;
using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;
using Cat.Commands.Products;


namespace nyanya.Internal.Models
{
    /// <summary>
    ///     ZCBUpdateShelvesRequest
    /// </summary>
    public class ZCBUpdateShelvesRequest : IRequestModel
    {
        /// <summary>
        ///     产品编号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ProductNo { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        ///     增加份额数
        /// </summary>
        [Required]
        [Range(1, 2000000000)]
        public int FinancingSumCount { get; set; }
        
        /// <summary>
        ///     每一份的单价
        /// </summary>
        [Range(1, 2000000000)]
        public int UnitPrice { get; set; }

        /// <summary>
        ///     下次开售时间
        /// </summary>
        [Required]
        public DateTime NextStartSellTime { get; set; }

        /// <summary>
        ///     下次售卖结束时间
        /// </summary>
        [Required]
        public DateTime NextEndSellTime { get; set; }

        /// <summary>
        ///     下次售卖年华收益
        /// </summary>
        [Required]
        public decimal NextYield { get; set; }

        /// <summary>
        ///     是否开启售卖 （0否 1是）
        /// </summary>
        [Required]
        [Range(0,1)]
        public int EnableSale { get; set; }

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
        ///     授权委托书
        /// </summary>
        [Required]
        [MaxLength(20000)]
        public string ConsignmentAgreement { get; set; }

        /// <summary>
        ///     投资协议
        /// </summary>
        [Required]
        [MaxLength(20000)]
        public string PledgeAgreement { get; set; }

        /// <summary>
        ///     产品下次可取现额度
        /// </summary>
        [Range(1, 2000000000)]
        public decimal PerRemainRedeemAmount { get; set; }
    }
    internal static partial class HitShelvesRequestExtensions
    {
        internal static ZCBUpdateShareCount ToZCBUpdateShareCount(this ZCBUpdateShelvesRequest request,string productIdentifier)
        {
            return new ZCBUpdateShareCount
            {
                FinancingSumCount = request.FinancingSumCount,
                ProductName = request.ProductName,
                ProductNo = request.ProductNo,
                SubProductNo = request.ProductNo + "_" + DateTime.Now.ToString("yyyyMMdd"),
                UnitPrice = request.UnitPrice,
                NextStartSellTime = request.NextStartSellTime,
                NextEndSellTime = request.NextEndSellTime,
                NextYield = request.NextYield,
                ProductIdentifier = productIdentifier,
                EnableSale = request.EnableSale,
                ConsignmentAgreementName = request.ConsignmentAgreementName,
                ConsignmentAgreement = request.ConsignmentAgreement,
                PledgeAgreementName = request.PledgeAgreementName,
                PledgeAgreement = request.PledgeAgreement,
                PerRemainRedeemAmount = request.PerRemainRedeemAmount
            };
        }

        internal static ZCBHitShelvesRequest ToZCBHitShelves(this ZCBUpdateShelvesRequest request)
        {
            return new ZCBHitShelvesRequest
            {
                ConsignmentAgreement = request.ConsignmentAgreement,
                ConsignmentAgreementName = request.ConsignmentAgreementName,
                EndorseImageLink = "http://1.1.1.1/",
                EndorseImageThumbnailLink = "http://1.1.1.1/",
                EndSellTime = request.NextEndSellTime,
                FinancingSumCount = request.FinancingSumCount,
                MaxShareCount = request.FinancingSumCount,
                MinShareCount = 1,
                Period = 30000,
                PledgeAgreementName = request.PledgeAgreementName,
                PledgeAgreement = request.PledgeAgreement,
                ProductName = request.ProductName,
                ProductNo = request.ProductNo,
                SubProductNo = request.ProductNo + "_" + DateTime.Now.ToString("yyyyMMdd"),
                ProductNumber = 1,
                RepaymentDeadline = DateTime.Parse("2099-12-31 00:00:00"),
                SettleDate = DateTime.Parse("2099-12-31 00:00:00"),
                StartSellTime = request.NextStartSellTime,
                UnitPrice = request.UnitPrice,
                ValueDateMode = ValueDateMode.T0,
                Yield = request.NextYield,
                PerRemainRedeemAmount = request.PerRemainRedeemAmount

            };
        }
    }
}
