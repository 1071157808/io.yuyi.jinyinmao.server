// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : CancelOrder.cs
// Created          : 2015-08-14  1:26
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-14  1:48
// ***********************************************************************
// <copyright file="CancelOrder.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     CancelOrder
    /// </summary>
    [Immutable]
    public class CancelOrder : Command
    {
        /// <summary>
        ///     订单Id
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        ///     产品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }
}