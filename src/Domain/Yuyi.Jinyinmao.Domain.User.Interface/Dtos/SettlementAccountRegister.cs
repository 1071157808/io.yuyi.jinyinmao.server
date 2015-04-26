// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-21  11:04 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  11:14 PM
// ***********************************************************************
// <copyright file="SettlementAccountRegister.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Dtos
{
    /// <summary>
    ///     SettlementAccountRegister.
    /// </summary>
    [Immutable]
    public class SettlementAccountRegister
    {
        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}
