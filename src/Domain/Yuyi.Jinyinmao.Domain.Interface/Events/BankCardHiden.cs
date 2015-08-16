// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-21  4:24 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-21  4:25 PM
// ***********************************************************************
// <copyright file="BankCardHiden.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using PostSharp.Patterns.Model;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     BankCardHiden.
    /// </summary>
    [Immutable]
    public class BankCardHiden : Event
    {
        /// <summary>
        ///     Gets or sets the bank card information.
        /// </summary>
        /// <value>The bank card information.</value>
        [Reference]
        public BankCardInfo BankCardInfo { get; set; }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        [Reference]
        public UserInfo UserInfo { get; set; }
    }
}