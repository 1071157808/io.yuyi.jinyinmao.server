// FileInformation: nyanya/nyanya.Cat/SendVeriCodeRequest.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/01   11:25 AM

using System.ComponentModel.DataAnnotations;
using Cat.Domain.Auth.Models;
using nyanya.AspDotNet.Common.Validation;

namespace nyanya.Cat.Models
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