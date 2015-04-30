// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  2:02 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  10:30 PM
// ***********************************************************************
// <copyright file="BankCardInfo.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     BankCardInfo.
    /// </summary>
    [Immutable]
    public class BankCardInfo
    {
        /// <summary>
        ///     Gets or sets the bank card no.
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the name of the bank.
        /// </summary>
        /// <value>The name of the bank.</value>
        public string BankName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance can be used for yilian.
        /// </summary>
        /// <value><c>true</c> if this instance can be used for yilian; otherwise, <c>false</c>.</value>
        public bool CanBeUsedForYilian { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value><c>true</c> if this instance is default; otherwise, <c>false</c>.</value>
        public bool IsDefault { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime VerifiedTime { get; set; }

        /// <summary>
        ///     Gets or sets the withdraw amount.
        /// </summary>
        /// <value>The withdraw amount.</value>
        public int WithdrawAmount { get; set; }
    }
}
