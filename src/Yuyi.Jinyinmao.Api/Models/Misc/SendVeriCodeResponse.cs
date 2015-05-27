// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-12  3:09 PM
// ***********************************************************************
// <copyright file="SendVeriCodeResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    /// Class SendVeriCodeResponse.
    /// </summary>
    public class SendVeriCodeResponse : IResponse
    {
        /// <summary>
        ///     今天剩余发送次数，若为-1，则今天不能再次发送该类型验证码
        /// </summary>
        /// <value>The remain count.</value>
        [Required, JsonProperty("remainCount")]
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次发送结果
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        [Required, JsonProperty("success")]
        public bool Success { get; set; }
    }

    /// <summary>
    ///     Class SendVeriCodeResponseEx.
    /// </summary>
    internal static class SendVeriCodeResponseEx
    {
        /// <summary>
        ///     To the response.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>SendVeriCodeResponse.</returns>
        internal static SendVeriCodeResponse ToResponse(this SendVeriCodeResult result) => new SendVeriCodeResponse
        {
            RemainCount = result.RemainCount,
            Success = result.Success
        };
    }
}