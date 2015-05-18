// FileInformation: nyanya/nyanya.Cat/ResetPaymentPasswordRequest.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   11:25 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     重置支付密码请求
    /// </summary>
    public class ResetPaymentPasswordRequest
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
        ///     用户设置的支付密码
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [MinLength(6, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [MaxLength(18, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [PaymentPasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        public string Password { get; set; }

        /// <summary>
        ///     验证码验证成功口令
        /// </summary>
        [Required]
        public string Token { get; set; }

        #endregion Public Properties
    }
}