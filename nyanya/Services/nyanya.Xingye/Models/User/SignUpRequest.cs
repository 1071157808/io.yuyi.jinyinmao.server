// FileInformation: nyanya/nyanya.Xingye/SignUpRequest.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/01   11:32 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     注册请求
    /// </summary>
    public class SignUpRequest : IRequestModel
    {
        /// <summary>
        ///     识别码
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        ///     用户设置的密码
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(18)]
        [PasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        public string Password { get; set; }

        /// <summary>
        ///     校验码验证成功口令
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}
