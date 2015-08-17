// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : SetSettleAccountTransactionResult.cs
// Created          : 2015-08-17  12:08
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  12:09
// ***********************************************************************
// <copyright file="SetSettleAccountTransactionResult.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     SetSettleAccountTransactionResult.
    /// </summary>
    [Immutable]
    public class SetSettleAccountTransactionResult : Command
    {
        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="SetJBYAccountTransactionResult" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the transacation identifier.
        /// </summary>
        /// <value>The transacation identifier.</value>
        public Guid TransacationId { get; set; }
    }
}