// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SettleAccountTransactionCanceled.cs
// Created          : 2015-08-05  10:17 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  10:18 PM
// ***********************************************************************
// <copyright file="SettleAccountTransactionCanceled.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     SettleAccountTransactionCanceled.
    /// </summary>
    [Immutable]
    public class SettleAccountTransactionCanceled : Event
    {
        /// <summary>
        ///     Gets or sets the transaction information.
        /// </summary>
        /// <value>The transaction information.</value>
        public SettleAccountTransactionInfo TransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}