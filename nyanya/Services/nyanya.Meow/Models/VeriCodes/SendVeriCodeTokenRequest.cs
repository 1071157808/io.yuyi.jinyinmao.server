using Cat.Domain.Auth.Models;
using nyanya.AspDotNet.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nyanya.Meow.Models.VeriCodes
{
    /// <summary>
    ///     验证码发送请求
    /// </summary>
    public class SendVeriCodeTokenRequest
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

        /// <summary>
        ///     验证手机用TOKEN
        /// </summary>
        public string Token { get; set; }
    }
}