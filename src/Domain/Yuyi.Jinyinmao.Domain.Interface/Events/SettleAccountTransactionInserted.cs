// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SettleAccountTransactionInserted.cs
// Created          : 2015-07-31  4:24 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  4:25 PM
// ***********************************************************************
// <copyright file="SettleAccountTransactionInserted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     SettleAccountTransactionInserted.
    /// </summary>
    [Immutable]
    public class SettleAccountTransactionInserted : Event
    {
        /// <summary>
        ///     Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        public SettleAccountTransactionInfo TransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}