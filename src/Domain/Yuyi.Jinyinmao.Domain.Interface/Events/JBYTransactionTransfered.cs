// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYTransactionTransfered.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  1:54
// ***********************************************************************
// <copyright file="JBYTransactionTransfered.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     OrderTransfered.
    /// </summary>
    [Immutable]
    public class JBYTransactionTransfered : Event
    {
        /// <summary>
        ///     Gets or sets the jby information.
        /// </summary>
        /// <value>The jby information.</value>
        public JBYAccountTransactionInfo JBYInfo { get; set; }

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