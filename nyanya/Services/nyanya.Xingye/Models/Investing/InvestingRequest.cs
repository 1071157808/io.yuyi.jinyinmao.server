// FileInformation: nyanya/nyanya.Xingye/InvestingRequest.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/01   11:33 AM

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.RequestModels;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     投资下单请求
    /// </summary>
    public class InvestingRequest : IRequestModel
    {
        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>
        ///     The bank card no.
        /// </value>
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the count.
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        [Range(1, 2000000000)]
        public int Count { get; set; }

        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>
        ///     The bank card no.
        /// </value>
        [Required]
        [StringLength(18, MinimumLength = 6, ErrorMessage = @"交易密码错误")]
        [Display(Name = @"交易密码")]
        public string PaymentPassword { get; set; }

        /// <summary>
        ///     Gets or sets the product no.
        /// </summary>
        /// <value>
        ///     The product no.
        /// </value>
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string ProductNo { get; set; }
    }
}