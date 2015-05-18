// FileInformation: nyanya/Services.WebAPI.V1.nyanya/SendVeriCodeRequest.cs
// CreatedTime: 2014/07/09   12:56 AM
// LastUpdatedTime: 2014/07/09   9:20 AM

using System.ComponentModel.DataAnnotations;
using Cqrs.Domain.Auth.Models;
using Services.WebAPI.Common.Validation;

namespace Services.WebAPI.V1.nyanya.Models.VeriCodes
{
    /// <summary>
    ///     验证码发送请求
    /// </summary>
    public class SendVeriCodeRequest
    {
        /// <summary>
        ///     手机号
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [CellphoneFormat(ErrorMessage = "手机号格式不正确")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     验证码用途类型。10 => 注册，20 => 忘记密码，30 => 修改密码
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [AvailableValues(VeriCode.VeriCodeType.SignUp, VeriCode.VeriCodeType.ResetLoginPassword, VeriCode.VeriCodeType.ResetPaymentPassword)]
        public VeriCode.VeriCodeType Type { get; set; }
    }
}