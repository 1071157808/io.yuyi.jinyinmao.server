// FileInformation: nyanya/Services.WebAPI.V1.nyanya/VerifyVeriCodeRequest.cs
// CreatedTime: 2014/07/15   3:35 PM
// LastUpdatedTime: 2014/07/16   4:54 PM

using System.ComponentModel.DataAnnotations;
using Cqrs.Domain.Auth.Models;
using Services.WebAPI.Common.Validation;

namespace Services.WebAPI.V1.nyanya.Models.VeriCodes
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