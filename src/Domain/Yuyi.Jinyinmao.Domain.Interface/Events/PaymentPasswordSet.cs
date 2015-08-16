// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : PaymentPasswordSet.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  1:33
// ***********************************************************************
// <copyright file="PaymentPasswordSet.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Diagnostics.CodeAnalysis;
using Orleans.Concurrency;
using PostSharp.Patterns.Model;
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
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        [Reference]
        public UserInfo UserInfo { get; set; }
    }
}