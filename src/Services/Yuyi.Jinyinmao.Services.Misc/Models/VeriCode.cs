// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  11:50 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  11:55 AM
// ***********************************************************************
// <copyright file="VeriCode.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Service.Models
{
    /// <summary>
    ///     VeriCode.
    /// </summary>
    public class VeriCode
    {
        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args { get; set; }

        /// <summary>
        ///     Gets or sets the build at.
        /// </summary>
        /// <value>The build at.</value>
        public DateTime BuildAt { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        ///     Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int ErrorCount { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the times.
        /// </summary>
        /// <value>The times.</value>
        public int Times { get; set; }

        /// <summary>
        ///     Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public int Type { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="VeriCode" /> is used.
        /// </summary>
        /// <value><c>true</c> if used; otherwise, <c>false</c>.</value>
        public bool Used { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="VeriCode" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }
    }
}