using nyanya.AspDotNet.Common.RequestModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     赎回本金请求
    /// </summary>
    public class RedeemPrincipalRequest : IRequestModel
    {
        /// <summary>
        ///    产品编号
        /// </summary>
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string ProductNo { get; set; }

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
        ///     赎回金额
        /// </summary>
        [Required]
        [Range(1, 50000)]
        public decimal RedeemPrincipal { get; set; }

        /// <summary>
        ///     支付密码
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(18)]
        public string PaymentPassword { get; set; }
    }
}