// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  3:20 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  12:23 AM
// ***********************************************************************
// <copyright file="PaymentPasswordSet.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     PaymentPasswordSet.
    /// </summary>
    [Immutable]
    public class PaymentPasswordSet : Event
    {
        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}