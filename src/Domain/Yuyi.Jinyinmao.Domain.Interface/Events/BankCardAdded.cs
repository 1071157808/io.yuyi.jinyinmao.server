// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : BankCardAdded.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  1:55
// ***********************************************************************
// <copyright file="BankCardAdded.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     BankCardAdded.
    /// </summary>
    [Immutable]
    public class BankCardAdded : Event
    {
        /// <summary>
        ///     Gets or sets the bank card information.
        /// </summary>
        /// <value>The bank card information.</value>
        public BankCardInfo BankCardInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}