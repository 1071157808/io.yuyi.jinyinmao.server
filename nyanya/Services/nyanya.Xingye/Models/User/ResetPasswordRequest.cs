// FileInformation: nyanya/nyanya.Xingye/ResetPasswordRequest.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/01   11:33 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     重置密码请求
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        ///     用户设置的密码
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_xy_PasswordFormatError")]
        [MinLength(6, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_xy_PasswordFormatError")]
        [MaxLength(18, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_xy_PasswordFormatError")]
        [PasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_xy_PasswordFormatError")]
        public string Password { get; set; }

        /// <summary>
        ///     校验码验证成功口令
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        [Required]
        public string CellPhone { get; set; }
    }
}
