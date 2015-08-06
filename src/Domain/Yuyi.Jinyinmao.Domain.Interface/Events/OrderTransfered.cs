// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : OrderTransfered.cs
// Created          : 2015-08-06  12:08 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-06  12:08 AM
// ***********************************************************************
// <copyright file="OrderTransfered.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    public class OrderTransfered : Event
    {
        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public OrderInfo OrderInfo { get; set; }

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