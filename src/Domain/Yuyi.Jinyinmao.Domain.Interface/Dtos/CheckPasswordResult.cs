// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  1:02 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-03  3:13 AM
// ***********************************************************************
// <copyright file="CheckPasswordResult.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     CheckPasswordResult.
    /// </summary>
    [Immutable]
    public class CheckPasswordResult
    {
        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     Gets or sets the remain count.
        /// </summary>
        /// <value>The remain count.</value>
        public int RemainCount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="CheckPasswordResult" /> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [user exist].
        /// </summary>
        /// <value><c>true</c> if [user exist]; otherwise, <c>false</c>.</value>
        public bool UserExist { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}