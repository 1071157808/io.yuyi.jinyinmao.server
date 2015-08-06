// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYAccountTransactionCanceled.cs
// Created          : 2015-08-05  10:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-05  10:19 PM
// ***********************************************************************
// <copyright file="JBYAccountTransactionCanceled.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYAccountTransactionCanceled.
    /// </summary>
    [Immutable]
    public class JBYAccountTransactionCanceled : Event
    {
        /// <summary>
        ///     Gets or sets the transaction information.
        /// </summary>
        /// <value>The transaction information.</value>
        public JBYAccountTransactionInfo TransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}