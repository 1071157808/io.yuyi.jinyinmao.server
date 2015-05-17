// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-25  3:22 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  12:14 AM
// ***********************************************************************
// <copyright file="PaymentPasswordReset.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     PaymentPasswordReset.
    /// </summary>
    [Immutable]
    public class PaymentPasswordReset : Event
    {
        /// <summary>
        /// Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        public UserInfo UserInfo { get; set; }
    }
}