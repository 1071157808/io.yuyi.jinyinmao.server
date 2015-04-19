// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-13  1:08 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-13  1:11 AM
// ***********************************************************************
// <copyright file="SignInRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.AspNet.Validations;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models.User
{
    /// <summary>
    ///     Class SignInRequest.
    /// </summary>
    public class SignInRequest : IRequest
    {
        /// <summary>
        ///     用户登录名
        /// </summary>
        [Required, CellphoneFormat, JsonProperty(PropertyName = "loginName")]
        public string LoginName { get; set; }

        /// <summary>
        ///     密码
        /// </summary>
        [Required, StringLength(18, MinimumLength = 6), SimplePasswordFormat, JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
