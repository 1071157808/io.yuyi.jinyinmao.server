// FileInformation: nyanya/Services.WebAPI.V1.nyanya/SetPaymentPasswordRequest.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   9:43 AM

using Infrastructure.Lib;
using Services.WebAPI.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Services.WebAPI.V1.nyanya.Models
{
    /// <summary>
    ///     设定支付密码请求模型
    /// </summary>
    public class SetPaymentPasswordRequest
    {
        #region Public Properties

        /// <summary>
        ///     密码
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [MinLength(6, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [MaxLength(18, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        [PaymentPasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PaymentPasswordFormatError")]
        public string Password { get; set; }

        #endregion Public Properties
    }
}