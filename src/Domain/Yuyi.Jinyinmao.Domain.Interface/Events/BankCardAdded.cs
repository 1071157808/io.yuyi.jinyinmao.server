// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-15  1:45 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-15  6:37 PM
// ***********************************************************************
// <copyright file="BankCardAdded.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using PostSharp.Patterns.Model;
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