// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-13  2:02 AM
// ***********************************************************************
// <copyright file="VerifyVeriCodeResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.Lib;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Models.Misc
{
    /// <summary>
    ///     Class VerifyVeriCodeResponse.
    /// </summary>
    public class VerifyVeriCodeResponse : IResponse
    {
        /// <summary>
        ///     剩余的验证次数，该次数不需要告知用户，若为 -1 ，则该验证码已失效。验证码过期等其他非异常情况也会返回 -1 。
        ///     若为 0 ，则该验证码失效，不能再进行验证。该值为 -1 或者 0 时，可以显示“请重新发送验证码”
        /// </summary>
        /// <value>The remain count.</value>
        [Required, JsonProperty(PropertyName = "remainCount")]
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次验证结果
        /// </summary>
        /// <value><c>true</c> if successed; otherwise, <c>false</c>.</value>
        [Required, JsonProperty(PropertyName = "successed")]
        public bool Successed { get; set; }

        /// <summary>
        ///     验证码验证后的token，若验证码验证成功，则该token为32位字符串，若验证失败，为空字符串
        /// </summary>
        /// <value>The token.</value>
        [Required, JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }

    /// <summary>
    ///     Class VerifyVeriCodeResponseEx.
    /// </summary>
    internal static class VerifyVeriCodeResponseEx
    {
        /// <summary>
        ///     To the response.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>VerifyVeriCodeResponse.</returns>
        internal static VerifyVeriCodeResponse ToResponse(this VerifyVeriCodeResult result)
        {
            return new VerifyVeriCodeResponse
            {
                RemainCount = result.RemainCount,
                Successed = result.Successed,
                Token = result.Token.IsNotNullOrEmpty() ? result.Token : ""
            };
        }
    }
}
