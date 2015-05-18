// FileInformation: nyanya/nyanya.Cat/SetPaymentPasswordRequest.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   11:25 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Cat.Models
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