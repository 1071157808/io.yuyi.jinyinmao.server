// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-03  10:38 PM
// ***********************************************************************
// <copyright file="IVeriCodeService.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yuyi.Jinyinmao.Service.Interface
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
        /// <param name="args">The arguments.</param>
        /// <returns>Task&lt;SendVeriCodeResult&gt;.</returns>
        Task<SendVeriCodeResult> SendAsync(string cellphone, VeriCodeType type, Dictionary<string, object> args);

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