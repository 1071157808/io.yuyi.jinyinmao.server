// FileInformation: nyanya/nyanya.Internal/BAHitShelvesRequest.cs
// CreatedTime: 2014/08/26   5:01 PM
// LastUpdatedTime: 2014/09/01   5:25 PM

using System;
using System.ComponentModel.DataAnnotations;
using Cat.Commands.Products;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Internal.Models
{
    /// <summary>
    ///     上架请求
    /// </summary>
    public class BAHitShelvesRequest : IRequestModel
    {
        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [MaxLength(80)]
        public string BillNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string BusinessLicense { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [MaxLength(20000)]
        public string ConsignmentAgreement { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string EndorseImageThumbnailLink { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [MaxLength(80)]
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum count.
        /// </summary>
        [Required]
        [Range(1, 2000000000)]
        public int FinancingSumCount { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum count.
        /// </summary>
        [Required]
        [Range(1, 2000000000)]
        public int MaxShareCount { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum count.
        /// </summary>
        [Required]
        [Range(1, 2000000000)]
        public int MinShareCount { get; set; }

        /// <summary>
        ///     Gets or sets the financing sum count.
        /// </summary>
        [Required]
        [Range(1, 36500)]
        public int Period { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        public string PledgeAgreement { get; set; }

        /// <summary>
        ///     Gets or sets the pre end sell time.
        /// </summary>
        /// <value>
        ///     The pre end sell time.
        /// </value>
        public DateTime? PreEndSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the pre start sell time.
        /// </summary>
        /// <value>
        ///     The pre start sell time.
        /// </value>
        public DateTime? PreStartSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        public string ProductName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        public string ProductNo { get; set; }

        /// <summary>
        ///     Gets or sets the product number.
        /// </summary>
        [Required]
        [Range(0, 1000000000)]
        public int ProductNumber { get; set; }

        /// <summary>
        ///     Gets or sets the repayment deadline.
        /// </summary>
        [Required]
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     Gets or sets the settle date.
        /// </summary>
        [Required]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     Gets or sets the start sell time.
        /// </summary>
        [Required]
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     Gets or sets the unit price.
        /// </summary>
        [Range(1, 2000000000)]
        public int UnitPrice { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Usage { get; set; }

        /// <summary>
        ///     Gets or sets the value date.
        /// </summary>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     Gets or sets the value date mode.
        /// </summary>
        [AvailableValues(ValueDateMode.FixedDate, ValueDateMode.T0, ValueDateMode.T1)]
        public ValueDateMode ValueDateMode { get; set; }

        /// <summary>
        ///     Gets or sets the yield.
        /// </summary>
        [Required]
        [Range(0, 100)]
        public decimal Yield { get; set; }

        /// <summary>
        /// 产品分类 10金银猫产品 20富滇产品 40阜新产品
        /// </summary>
        public ProductCategory ProductCategory { get; set; }
    }

    internal static partial class HitShelvesRequestExtensions
    {
        internal static LaunchBAProduct ToLaunchBAProduct(this BAHitShelvesRequest request)
        {
            return new LaunchBAProduct
            {
                BankName = request.BankName,
                BillNo = request.BillNo,
                BusinessLicense = request.BusinessLicense,
                ConsignmentAgreement = request.ConsignmentAgreement,
                EndorseImageLink = request.EndorseImageLink,
                EndorseImageThumbnailLink = request.EndorseImageThumbnailLink,
                EndSellTime = request.EndSellTime,
                EnterpriseName = request.EnterpriseName,
                FinancingSumCount = request.FinancingSumCount,
                MaxShareCount = request.MaxShareCount,
                MinShareCount = request.MinShareCount,
                Period = request.Period,
                PledgeAgreement = request.PledgeAgreement,
                PreEndSellTime = request.PreEndSellTime,
                PreStartSellTime = request.PreStartSellTime,
                ProductName = request.ProductName,
                ProductNo = request.ProductNo,
                ProductNumber = request.ProductNumber,
                RepaymentDeadline = request.RepaymentDeadline,
                SettleDate = request.SettleDate,
                StartSellTime = request.StartSellTime,
                UnitPrice = request.UnitPrice,
                Usage = request.Usage,
                ValueDate = request.ValueDate,
                ValueDateMode = request.ValueDateMode,
                Yield = request.Yield,
                ProductCategory = request.ProductCategory == ProductCategory.FUDIAN ? ProductCategory.FUDIAN : request.ProductCategory != ProductCategory.FUXIN ? ProductCategory.JINYINMAO : request.ProductCategory
            };
        }
    }
}