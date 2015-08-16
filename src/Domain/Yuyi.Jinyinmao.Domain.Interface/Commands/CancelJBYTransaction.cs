// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : CancelJBYTransaction.cs
// Created          : 2015-08-14  1:26
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  0:52
// ***********************************************************************
// <copyright file="CancelJBYTransaction.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     CancelJBYTransaction.
    /// </summary>
    [Immutable]
    public class CancelJBYTransaction : Command
    {
        /// <summary>
        ///     流水Id
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }
}