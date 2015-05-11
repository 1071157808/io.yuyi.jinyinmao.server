// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  9:53 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  9:55 PM
// ***********************************************************************
// <copyright file="JBYPurchased.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYPurchased.
    /// </summary>
    [Immutable]
    public class JBYPurchased : Event
    {
        /// <summary>
        ///     Gets or sets the jby transcation.
        /// </summary>
        /// <value>The jby transcation.</value>
        public TranscationInfo JBYTranscation { get; set; }

        /// <summary>
        ///     Gets or sets the settle transcation.
        /// </summary>
        /// <value>The settle transcation.</value>
        public TranscationInfo SettleTranscation { get; set; }
    }
}