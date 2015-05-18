// FileInformation: nyanya/Services.WebAPI.V1.nyanya/InvestingRequest.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/11   12:48 PM

using System.ComponentModel.DataAnnotations;
using Services.WebAPI.Common.RequestModels;

namespace Services.WebAPI.V1.nyanya.Models
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
        [MinLength(6)]
        [MaxLength(18)]
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