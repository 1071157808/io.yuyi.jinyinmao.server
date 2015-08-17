// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : Sign.cs
// Created          : 2015-08-17  20:03
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  20:04
// ***********************************************************************
// <copyright file="Sign.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     Sign.
    /// </summary>
    [Immutable]
    public class Sign : Command
    {
        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserIdentifier { get; set; }
    }
}