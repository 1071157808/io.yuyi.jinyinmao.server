// FileInformation: nyanya/nyanya.Meow/ResetPasswordRequest.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   11:34 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     重置密码请求
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        ///     用户设置的密码
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        [MinLength(6, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        [MaxLength(18, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        [PasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        public string Password { get; set; }

        /// <summary>
        ///     验证码验证成功口令
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}