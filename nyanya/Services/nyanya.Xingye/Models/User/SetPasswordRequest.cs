// FileInformation: nyanya/nyanya.Xingye/SetPasswordRequest.cs
// CreatedTime: 2014/09/03   16:08 PM
// LastUpdatedTime: 2014/09/03   16:08 PM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     置登陆密码请求
    /// </summary>
    public class SetPasswordRequest
    {
        /// <summary>
        ///     用户设置的密码
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_xy_PasswordFormatError")]
        [MinLength(6, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_xy_PasswordFormatError")]
        [MaxLength(18, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_xy_PasswordFormatError")]
        [PasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_xy_PasswordFormatError")]
        public string Password { get; set; }
    }
}