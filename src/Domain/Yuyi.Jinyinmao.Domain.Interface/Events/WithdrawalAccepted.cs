// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  10:46 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  10:46 PM
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
    /// WithdrawalAccepted.
    /// </summary>
    [Immutable]
    public class WithdrawalAccepted : Event
    {
        /// <summary>
        /// Gets or sets the charge transcation.
        /// </summary>
        /// <value>The charge transcation.</value>
        public TranscationInfo ChargeTranscation { get; set; }

        /// <summary>
        /// Gets or sets the withdrawal transcation.
        /// </summary>
        /// <value>The withdrawal transcation.</value>
        public TranscationInfo WithdrawalTranscation { get; set; }
    }
}