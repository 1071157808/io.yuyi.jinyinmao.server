// FileInformation: nyanya/nyanya.Cat/SignInRequest.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   11:25 AM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Cat.Models
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
        [Required(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        [MinLength(6, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        [MaxLength(18, ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        [PasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        public string Password { get; set; }

        #endregion Public Properties
    }
}