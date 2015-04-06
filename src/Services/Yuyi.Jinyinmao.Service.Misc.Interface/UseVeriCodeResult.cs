// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  4:01 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  6:17 PM
// ***********************************************************************
// <copyright file="UseVeriCodeResult.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Service.Interface
{
    /// <summary>
    ///     Class UseVeriCodeResult.
    /// </summary>
    public class UseVeriCodeResult
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="UseVeriCodeResult" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }
    }
}
