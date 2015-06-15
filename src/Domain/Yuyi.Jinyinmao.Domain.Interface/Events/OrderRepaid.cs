// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:35 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  7:08 PM
// ***********************************************************************
// <copyright file="OrderRepaid.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     OrderRepaid.
    /// </summary>
    [Immutable]
    public class OrderRepaid : Event
    {
        /// <summary>
        ///     Gets or sets the interest transcation information.
        /// </summary>
        /// <value>The interest transcation information.</value>
        public SettleAccountTranscationInfo InterestTranscationInfo { get; set; }

        /// <summary>
        ///     Gets or sets the order information.
        /// </summary>
        /// <value>The order information.</value>
        public OrderInfo OrderInfo { get; set; }

        /// <summary>
        ///     Gets or sets the pri int sum amount.
        /// </summary>
        /// <value>The pri int sum amount.</value>
        public long PriIntSumAmount { get; set; }

        /// <summary>
        ///     Gets or sets the principal transcation information.
        /// </summary>
        /// <value>The principal transcation information.</value>
        public SettleAccountTranscationInfo PrincipalTranscationInfo { get; set; }

        /// <summary>
        ///     Gets or sets the repaid time.
        /// </summary>
        /// <value>The repaid time.</value>
        public DateTime RepaidTime { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}