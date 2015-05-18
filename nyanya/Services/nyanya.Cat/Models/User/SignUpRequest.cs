// FileInformation: nyanya/nyanya.Cat/SignUpRequest.cs
// CreatedTime: 2015/03/05   9:54 PM
// LastUpdatedTime: 2015/03/08   5:05 PM

using Infrastructure.Lib;
using nyanya.AspDotNet.Common.RequestModels;
using nyanya.AspDotNet.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace nyanya.Cat.Models
{
    /// <summary>
    ///     注册请求
    /// </summary>
    public class SignUpRequest : IRequestModel
    {
        /// <summary>
        ///     客户端类型
        /// </summary>
        public long ClientType { get; set; }

        /// <summary>
        ///     识别码
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        ///     活动编号
        /// </summary>
        public long ContractId { get; set; }

        /// <summary>
        ///     FlgMoreI1
        /// </summary>
        public long FlgMoreI1 { get; set; }

        /// <summary>
        ///     FlgMoreI2
        /// </summary>
        public long FlgMoreI2 { get; set; }

        /// <summary>
        ///     FlgMore1
        /// </summary>
        public string FlgMoreS1 { get; set; }

        /// <summary>
        ///     FlgMoreS2
        /// </summary>
        public string FlgMoreS2 { get; set; }

        /// <summary>
        ///     邀请码
        /// </summary>
        public string InviteBy { get; set; }

        /// <summary>
        ///     用户设置的密码
        /// </summary>
        [Required]
        [MinLength(6)]
        [MaxLength(18)]
        [PasswordFormat(ErrorMessageResourceType = typeof(NyanyaResources), ErrorMessageResourceName = "RequestValidationMessage_PasswordFormatError")]
        public string Password { get; set; }

        /// <summary>
        ///     验证码验证成功口令
        /// </summary>
        [Required]
        public string Token { get; set; }
    }
}
