// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYWithdrawalResulted.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  1:55
// ***********************************************************************
// <copyright file="JBYWithdrawalResulted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYWithdrawalResulted.
    /// </summary>
    [Immutable]
    public class JBYWithdrawalResulted : Event
    {
        /// <summary>
        ///     Gets or sets the jby account transaction information.
        /// </summary>
        /// <value>The jby account transaction information.</value>
        public JBYAccountTransactionInfo JBYAccountTransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the settle account transaction information.
        /// </summary>
        /// <value>The settle account transaction information.</value>
        public SettleAccountTransactionInfo SettleAccountTransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}