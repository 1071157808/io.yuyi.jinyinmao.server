// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  11:21 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  12:14 AM
// ***********************************************************************
// <copyright file="AddBankCardResulted.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     AddBankCardResulted.
    /// </summary>
    [Immutable]
    public class AddBankCardResulted : Event
    {
        /// <summary>
        ///     银行卡号
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        /// <value>The name of the bank.</value>
        public string BankName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be used by yilian.
        /// </summary>
        /// <value><c>true</c> if this instance can be used by yilian; otherwise, <c>false</c>.</value>
        public bool CanBeUsedByYilian { get; set; }

        /// <summary>
        ///     Gets or sets the cellphone.
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     银行所属城市
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value><c>true</c> if this instance is default; otherwise, <c>false</c>.</value>
        public bool IsDefault { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AddBankCardResulted" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        /// Gets or sets the tran desc.
        /// </summary>
        /// <value>The tran desc.</value>
        public string TranDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AddBankCardResulted"/> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        /// Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime VerifiedTime { get; set; }
    }
}