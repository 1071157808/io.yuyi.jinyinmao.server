// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  2:02 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  2:06 AM
// ***********************************************************************
// <copyright file="ResetPasswordRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.AspNet.Validations;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     ResetPasswordRequest.
    /// </summary>
    public class ResetPasswordRequest : IRequest
    {
        /// <summary>
        ///     用户设置的密码
        /// </summary>
        [Required, SimplePasswordFormat, JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        /// <summary>
        ///     验证码验证成功口令
        /// </summary>
        [Required, JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
