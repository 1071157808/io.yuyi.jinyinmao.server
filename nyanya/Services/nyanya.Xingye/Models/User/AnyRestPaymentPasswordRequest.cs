using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Models
{
    /// <summary>
    /// 匿名重置支付密码
    /// </summary>
    public class AnyRestPaymentPasswordRequest
    {
        #region Public Properties

        /// <summary>
        ///     默认银行卡号
        /// </summary>
        [Required]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     用户证件号
        /// </summary>
        [Required]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        [Required]
        public string CellphoneNo { get; set; }

        /// <summary>
        ///     用户设置的支付密码
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [MinLength(6, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [MaxLength(18, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [PaymentPasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        public string Password { get; set; }

        /// <summary>
        ///     校验码验证成功口令
        /// </summary>
        [Required]
        public string Token { get; set; }

        #endregion Public Properties
    }
}
