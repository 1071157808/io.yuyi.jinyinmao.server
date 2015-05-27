// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  11:19 AM
// ***********************************************************************
// <copyright file="SignInRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.AspNet.Validations;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     Class SignInRequest.
    /// </summary>
    public class SignInRequest : IRequest
    {
        /// <summary>
        ///     用户登录名
        /// </summary>
        [Required, CellphoneFormat, JsonProperty("loginName")]
        public string LoginName { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), SimplePasswordFormat, JsonProperty("password")]
        public string Password { get; set; }
    }
}