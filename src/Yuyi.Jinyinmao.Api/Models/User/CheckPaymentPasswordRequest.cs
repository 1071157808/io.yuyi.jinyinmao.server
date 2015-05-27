// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-08  1:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-08  1:39 PM
// ***********************************************************************
// <copyright file="CheckPaymentPasswordRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models.User
{
    /// <summary>
    ///     CheckPaymentPasswordRequest.
    /// </summary>
    public class CheckPaymentPasswordRequest : IRequest
    {
        /// <summary>
        ///     支付密码
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), JsonProperty("password")]
        public string Password { get; set; }
    }
}