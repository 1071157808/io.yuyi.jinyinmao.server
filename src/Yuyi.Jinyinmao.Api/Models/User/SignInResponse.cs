// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-19  5:34 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-25  1:59 AM
// ***********************************************************************
// <copyright file="SignInResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Service.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     Class SignInResponse.
    /// </summary>
    public class SignInResponse : IResponse
    {
        /// <summary>
        ///     账户是否被锁定
        /// </summary>
        [Required, JsonProperty("lock")]
        public bool Lock { get; set; }

        /// <summary>
        ///     剩余尝试登录次数，如果账户被锁定，只能通过修改登录密码的方式进行重置
        /// </summary>
        [Required, JsonProperty("remainCount")]
        public int RemainCount { get; set; }

        /// <summary>
        ///     登陆结果
        /// </summary>
        [Required, JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        ///     用户是否存在
        /// </summary>
        [Required, JsonProperty("userExist")]
        public bool UserExist { get; set; }
    }

    internal static class SignInResultEx
    {
        internal static SignInResponse ToResponse(this SignInResult result)
        {
            return new SignInResponse
            {
                Lock = result.Lock,
                RemainCount = result.RemainCount,
                Success = result.Success,
                UserExist = result.UserExist
            };
        }
    }
}
