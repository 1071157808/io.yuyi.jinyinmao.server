// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  5:19 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-09  3:56 PM
// ***********************************************************************
// <copyright file="OrderPaid.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     OrderPaid.
    /// </summary>
    [Immutable]
    public class OrderPaid : Event
    {
        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public OrderInfo Order { get; set; }

        /// <summary>
        ///     Gets or sets the transcation.
        /// </summary>
        /// <value>The transcation.</value>
        public TranscationInfo Transcation { get; set; }
    }
}