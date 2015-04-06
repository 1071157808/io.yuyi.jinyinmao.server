// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  3:51 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  6:17 PM
// ***********************************************************************
// <copyright file="IVeriCodeService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Threading.Tasks;
using Yuyi.Jinyinmao.Service.Interface;

namespace Yuyi.Jinyinmao.Service.Misc.Interface
{
    /// <summary>
    ///     Interface IVeriCodeService
    /// </summary>
    public interface IVeriCodeService
    {
        /// <summary>
        ///     Sends the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <returns>Task&lt;SendVeriCodeResult&gt;.</returns>
        Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCodeType type, string message = "验证码:{0}，{1}分钟内有效");

        /// <summary>
        ///     Uses the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;UseVeriCodeResult&gt;.</returns>
        Task<UseVeriCodeResult> UseAsync(string token, VeriCodeType type);

        /// <summary>
        ///     Verifies the asynchronous.
        /// </summary>
        /// <param name="cellphone">The cellphone.</param>
        /// <param name="code">The code.</param>
        /// <param name="type">The type.</param>
        /// <returns>Task&lt;VerifyVeriCodeResult&gt;.</returns>
        Task<VerifyVeriCodeResult> VerifyAsync(string cellphone, string code, VeriCodeType type);
    }
}
