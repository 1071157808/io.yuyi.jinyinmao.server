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
        ///     Gets or sets the jby account transcation information.
        /// </summary>
        /// <value>The jby account transcation information.</value>
        public JBYAccountTranscationInfo JBYAccountTranscationInfo { get; set; }

        /// <summary>
        ///     Gets or sets the settle account transcation information.
        /// </summary>
        /// <value>The settle account transcation information.</value>
        public SettleAccountTranscationInfo SettleAccountTranscationInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}