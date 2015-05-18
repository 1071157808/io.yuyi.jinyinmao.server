// FileInformation: nyanya/nyanya.Xingye/SignInRequest.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/01   11:33 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     登陆请求
    /// </summary>
    public class SignInRequest
    {
        #region Public Properties

        /// <summary>
        ///     用户名
        /// </summary>
        [Required]
        [CellphoneFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequetValidationMessage_CellphoneFormatError")]
        public string Name { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError_SignIn")]
        [MinLength(6, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError_SignIn")]
        [MaxLength(18, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError_SignIn")]
        [PasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError_SignIn")]
        public string Password { get; set; }

        #endregion Public Properties
    }
}