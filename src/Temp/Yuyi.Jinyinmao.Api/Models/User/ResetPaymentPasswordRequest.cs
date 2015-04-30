// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  11:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  11:56 AM
// ***********************************************************************
// <copyright file="ResetPaymentPasswordRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Api.Validations;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     ResetPaymentPasswordRequest.
    /// </summary>
    public class ResetPaymentPasswordRequest : IRequest
    {
        /// <summary>
        ///     用户证件号
        /// </summary>
        [JsonProperty(PropertyName = "credentialNo")]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     用户设置的支付密码
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), PaymentPasswordFormat, JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        /// <summary>
        ///     验证码验证成功口令
        /// </summary>
        [Required, StringLength(32, MinimumLength = 32), JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        /// <summary>
        ///     用户真实姓名
        /// </summary>
        [JsonProperty(PropertyName = "userRealName")]
        public string UserRealName { get; set; }
    }
}
