using Cat.Domain.Auth.Models;
using nyanya.AspDotNet.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nyanya.Cat.Models.VeriCodes
{
    public class VerifyVeriCodePhoneRequest
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
        [MinLength(1)]
        public string Result { get; set; }
    }
}