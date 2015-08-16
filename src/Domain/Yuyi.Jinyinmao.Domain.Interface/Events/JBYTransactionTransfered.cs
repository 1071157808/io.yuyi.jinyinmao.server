// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYTransactionTransfered.cs
// Created          : 2015-08-06  12:43 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-06  12:44 AM
// ***********************************************************************
// <copyright file="JBYTransactionTransfered.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using PostSharp.Patterns.Model;
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
        [Reference]
        public JBYAccountTransactionInfo JBYInfo { get; set; }

        /// <summary>
        ///     Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [Reference]
        public SettleAccountTransactionInfo TransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        [Reference]
        public UserInfo UserInfo { get; set; }
    }
}