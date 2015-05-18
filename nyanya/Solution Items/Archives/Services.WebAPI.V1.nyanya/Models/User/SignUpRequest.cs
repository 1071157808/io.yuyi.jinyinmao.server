// FileInformation: nyanya/Services.WebAPI.V1.nyanya/SignUpRequest.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/08/12   9:43 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using Services.WebAPI.Common.Validation;

namespace Services.WebAPI.V1.nyanya.Models.Users
{
    /// <summary>
    ///     注册请求
    /// </summary>
    public class SignUpRequest
    {
        /// <summary>
        /// 识别码
        /// </summary>
        [Required]
        public string Code { get; set; }

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