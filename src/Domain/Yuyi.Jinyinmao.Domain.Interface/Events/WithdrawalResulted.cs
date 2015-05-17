// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  2:05 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  3:38 AM
// ***********************************************************************
// <copyright file="WithdrawalResulted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     WithdrawalResulted.
    /// </summary>
    public class WithdrawalResulted : Event
    {
        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        ///     Gets or sets the withdrawal transcation information.
        /// </summary>
        /// <value>The withdrawal transcation information.</value>
        public SettleAccountTranscationInfo WithdrawalTranscationInfo { get; set; }
    }
}