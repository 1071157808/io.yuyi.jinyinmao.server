// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-13  2:02 AM
// ***********************************************************************
// <copyright file="VerifyVeriCodeRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.AspNet.Validations;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     Class VerifyVeriCodeRequest.
    /// </summary>
    public class VerifyVeriCodeRequest : IRequest
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        [Required, CellphoneFormat, JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     验证码，用于验证，最短6位
        /// </summary>
        [Required, MinLength(6), JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        ///     验证码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码
        /// </summary>
        [Required, AvailableValues(VeriCodeType.SignUp, VeriCodeType.ResetLoginPassword, VeriCodeType.ResetPaymentPassword), JsonProperty("type")]
        public VeriCodeType Type { get; set; }
    }
}
