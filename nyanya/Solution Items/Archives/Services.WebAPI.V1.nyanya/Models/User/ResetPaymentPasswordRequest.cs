// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ResetPaymentPasswordRequest.cs
// CreatedTime: 2014/08/10   6:57 PM
// LastUpdatedTime: 2014/08/13   2:15 PM

using Infrastructure.Lib;
using Services.WebAPI.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.V1.nyanya.Models
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