// FileInformation: nyanya/nyanya.Xingye/VerifyVeriCodeRequest.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:28 PM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.Validation;
using Xingye.Domain.Auth.Models;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     校验码验证请求
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
        ///     校验码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [MinLength(6)]
        public string Code { get; set; }

        /// <summary>
        ///     校验码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码, 40 => 未激活用户注册
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [AvailableValues(VeriCode.VeriCodeType.SignUp, VeriCode.VeriCodeType.ResetLoginPassword, VeriCode.VeriCodeType.ResetPaymentPassword, VeriCode.VeriCodeType.TempSignUp)]
        public VeriCode.VeriCodeType Type { get; set; }
    }
}
