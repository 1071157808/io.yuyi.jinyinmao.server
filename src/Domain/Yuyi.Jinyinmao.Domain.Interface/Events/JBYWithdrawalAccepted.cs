// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-18  11:03 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  11:04 PM
// ***********************************************************************
// <copyright file="JBYWithdrawalAccepted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYWithdrawalAccepted.
    /// </summary>
    [Immutable]
    public class JBYWithdrawalAccepted : Event
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