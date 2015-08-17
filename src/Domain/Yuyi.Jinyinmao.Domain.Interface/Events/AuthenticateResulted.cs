// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : AuthenticateResulted.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  1:55
// ***********************************************************************
// <copyright file="AuthenticateResulted.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     AuthenticateResulted.
    /// </summary>
    [Immutable]
    public class AuthenticateResulted : Event
    {
        /// <summary>
        ///     Gets or sets the bank card information.
        /// </summary>
        /// <value>The bank card information.</value>
        public BankCardInfo BankCardInfo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="AuthenticateResulted" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the transaction desc.
        /// </summary>
        /// <value>The tran desc.</value>
        public string TranDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}