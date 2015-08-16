// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYAccountTransactionInserted.cs
// Created          : 2015-08-03  11:23 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-03  11:24 AM
// ***********************************************************************
// <copyright file="JBYAccountTransactionInserted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using PostSharp.Patterns.Model;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYAccountTransactionInserted.
    /// </summary>
    [Immutable]
    public class JBYAccountTransactionInserted : Event
    {
        /// <summary>
        ///     Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [Reference]
        public JBYAccountTransactionInfo TransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        [Reference]
        public UserInfo UserInfo { get; set; }
    }
}