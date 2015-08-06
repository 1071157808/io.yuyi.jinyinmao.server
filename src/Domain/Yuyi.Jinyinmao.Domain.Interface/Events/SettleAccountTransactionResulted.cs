// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SettleAccountTransactionResulted.cs
// Created          : 2015-08-05  5:50 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  5:51 PM
// ***********************************************************************
// <copyright file="SettleAccountTransactionResulted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    /// SettleAccountTransactionResulted.
    /// </summary>
    [Immutable]
    public class SettleAccountTransactionResulted : Event
    {
        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SettleAccountTransactionResulted" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the transaction information.
        /// </summary>
        /// <value>The transaction information.</value>
        public SettleAccountTransactionInfo TransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}