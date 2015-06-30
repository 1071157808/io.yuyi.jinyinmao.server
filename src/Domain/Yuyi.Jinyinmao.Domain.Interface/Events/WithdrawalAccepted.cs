// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  10:46 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  12:30 AM
// ***********************************************************************
// <copyright file="WithdrawalAccepted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     WithdrawalAccepted.
    /// </summary>
    [Immutable]
    public class WithdrawalAccepted : Event
    {
        /// <summary>
        ///     Gets or sets the charge transaction.
        /// </summary>
        /// <value>The charge transaction.</value>
        public SettleAccountTransactionInfo ChargeTransaction { get; set; }

        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        ///     Gets or sets the withdrawal transaction.
        /// </summary>
        /// <value>The withdrawal transaction.</value>
        public SettleAccountTransactionInfo WithdrawalTransaction { get; set; }
    }
}