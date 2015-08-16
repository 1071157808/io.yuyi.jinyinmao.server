// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-19  12:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  12:01 AM
// ***********************************************************************
// <copyright file="JBYWithdrawalResulted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using PostSharp.Patterns.Model;
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
        [Reference]
        public JBYAccountTransactionInfo JBYAccountTransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the settle account transaction information.
        /// </summary>
        /// <value>The settle account transaction information.</value>
        [Reference]
        public SettleAccountTransactionInfo SettleAccountTransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        [Reference]
        public UserInfo UserInfo { get; set; }
    }
}