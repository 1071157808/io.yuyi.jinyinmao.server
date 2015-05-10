// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  3:59 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  4:01 PM
// ***********************************************************************
// <copyright file="VerifyVeriCodeResult.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Class VerifyVeriCodeResult.
    /// </summary>
    public class VerifyVeriCodeResult
    {
        /// <summary>
        ///     Gets or sets the remain count.
        /// </summary>
        /// <value>The remain count.</value>
        public int RemainCount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="VerifyVeriCodeResult" /> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        /// <summary>
        ///     Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }
    }
}