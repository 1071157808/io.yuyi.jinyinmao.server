// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  7:37 PM
// ***********************************************************************
// <copyright file="CheckPaymentPasswordResult.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     CheckPaymentPasswordResult.
    /// </summary>
    public class CheckPaymentPasswordResult
    {
        /// <summary>
        ///     Gets a value indicating whether this <see cref="CheckPaymentPasswordResult" /> is lock.
        /// </summary>
        /// <value><c>true</c> if lock; otherwise, <c>false</c>.</value>
        public bool Lock
        {
            get { return this.RemainCount < 1; }
        }

        /// <summary>
        ///     Gets or sets the remain count.
        /// </summary>
        /// <value>The remain count.</value>
        public int RemainCount { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="CheckPaymentPasswordResult" /> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }
    }
}