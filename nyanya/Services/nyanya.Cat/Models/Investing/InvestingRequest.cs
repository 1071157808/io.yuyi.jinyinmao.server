// FileInformation: nyanya/nyanya.Cat/InvestingRequest.cs
// CreatedTime: 2015/03/04   6:31 PM
// LastUpdatedTime: 2015/03/10   3:08 PM

using nyanya.AspDotNet.Common.RequestModels;
using System.ComponentModel.DataAnnotations;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     投资下单请求
    /// </summary>
    public class InvestingRequest : IRequestModel
    {
        /// <summary>
        ///     Gets or sets the Activity no.
        /// </summary>
        /// <value>
        ///     The Activity no.
        /// </value>
        public decimal ActivityNo { get; set; }

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

        public long ClientType { get; set; }

        /// <summary>
        ///     Gets or sets the count.
        /// </summary>
        /// <value>
        ///     The count.
        /// </value>
        [Range(1, 2000000000)]
        public int Count { get; set; }

        public long FlgMoreI1 { get; set; }

        public long FlgMoreI2 { get; set; }

        public string FlgMoreS1 { get; set; }

        public string FlgMoreS2 { get; set; }

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
