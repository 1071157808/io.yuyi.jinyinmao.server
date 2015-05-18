// FileInformation: nyanya/nyanya.Xingye/SendVeriCodeRequest.cs
// CreatedTime: 2014/09/01   10:17 AM
// LastUpdatedTime: 2014/09/02   3:28 PM

using System.ComponentModel.DataAnnotations;
using nyanya.AspDotNet.Common.Validation;
using Xingye.Domain.Auth.Models;

namespace nyanya.Xingye.Models
{
    /// <summary>
    ///     校验码发送请求
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
        ///     校验码用途类型。10 => 注册，20 => 忘记密码，30 => 修改密码, 40 => 未激活用户注册
        /// </summary>
        [Required(ErrorMessage = "必填")]
        [AvailableValues(VeriCode.VeriCodeType.SignUp, VeriCode.VeriCodeType.ResetLoginPassword, VeriCode.VeriCodeType.ResetPaymentPassword, VeriCode.VeriCodeType.TempSignUp)]
        public VeriCode.VeriCodeType Type { get; set; }
    }
}
