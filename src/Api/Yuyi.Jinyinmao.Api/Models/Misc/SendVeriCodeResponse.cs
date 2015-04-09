// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  10:32 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-07  1:20 AM
// ***********************************************************************
// <copyright file="SendVeriCodeResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.ResponseModels;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Api.Models.Misc
{
    /// <summary>
    ///     验证码发送响应
    /// </summary>
    public class SendVeriCodeResponse : IResponse
    {
        /// <summary>
        ///     今天剩余发送次数，若为-1，则今天不能再次发送该类型验证码
        /// </summary>
        /// <value>The remain count.</value>
        [Required]
        public int RemainCount { get; set; }

        /// <summary>
        ///     本次发送结果
        /// </summary>
        /// <value><c>true</c> if successed; otherwise, <c>false</c>.</value>
        [Required]
        public bool Successed { get; set; }
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
        internal static SendVeriCodeResponse ToResponse(this SendVeriCodeResult result)
        {
            return new SendVeriCodeResponse
            {
                RemainCount = result.RemainCount,
                Successed = result.Successed
            };
        }
    }
}
