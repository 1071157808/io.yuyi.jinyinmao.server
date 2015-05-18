// FileInformation: nyanya/nyanya.Meow/VerifyVeriCodeRequest.cs
// CreatedTime: 2014/08/29   2:26 PM
// LastUpdatedTime: 2014/09/01   5:26 PM

using System.ComponentModel.DataAnnotations;
using Cat.Domain.Auth.Models;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Meow.Models
{
    /// <summary>
    ///     验证码验证请求
    /// </summary>
    public class VerifyVeriCodeRequest
    {
        /// <summary>
        ///     手机号
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [CellphoneFormat(ErrorMessage = "手机号格式不正确")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     验证码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [MinLength(6)]
        public string Code { get; set; }

        /// <summary>
        ///     验证码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [AvailableValues(VeriCode.VeriCodeType.SignUp, VeriCode.VeriCodeType.ResetLoginPassword, VeriCode.VeriCodeType.ResetPaymentPassword)]
        public VeriCode.VeriCodeType Type { get; set; }
    }
}