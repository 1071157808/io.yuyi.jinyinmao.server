// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : OrderRepay.cs
// Created          : 2015-08-13  23:37
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-19  19:35
// ***********************************************************************
// <copyright file="OrderRepay.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     OrderRepay
    /// </summary>
    [Immutable]
    public class OrderRepay : Command
    {
        /// <summary>
        ///     订单Id
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        ///     还款时间
        /// </summary>
        public DateTime RepayTime { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }
}