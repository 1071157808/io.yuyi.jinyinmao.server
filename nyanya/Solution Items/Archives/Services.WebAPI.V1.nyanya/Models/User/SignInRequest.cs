// FileInformation: nyanya/Services.WebAPI.V1.nyanya/SignInRequest.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/13   12:29 PM

using System.ComponentModel.DataAnnotations;
using Infrastructure.Lib;
using Services.WebAPI.Common.Validation;

namespace Services.WebAPI.V1.nyanya.Models
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