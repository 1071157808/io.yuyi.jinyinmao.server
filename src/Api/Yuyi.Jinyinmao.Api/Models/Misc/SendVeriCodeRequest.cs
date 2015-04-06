// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  10:13 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  10:22 PM
// ***********************************************************************
// <copyright file="SendVeriCodeRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.RequestModels;
using Moe.AspNet.Validations;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Models.Misc
{
    /// <summary>
    ///     验证码发送请求
    /// </summary>
    public class SendVeriCodeRequest : IRequest
    {
        /// <summary>
        ///     手机号码，验证使用正则表达式：^(13|14|15|17|18)\d{9}$
        /// </summary>
        [Required, CellphoneFormat]
        public string Cellphone { get; set; }

        /// <summary>
        ///     验证码用途类型。10 => 注册，20 => 重置登录密码，30 => 重置支付密码
        /// </summary>
        [Required, AvailableValues(VeriCodeType.SignUp, VeriCodeType.ResetLoginPassword, VeriCodeType.ResetPaymentPassword)]
        public VeriCodeType Type { get; set; }
    }
}
