// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-05  1:01 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-05  1:07 AM
// ***********************************************************************
// <copyright file="AddExtraInterest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     AddExtraInterest.
    /// </summary>
    [Immutable]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class AddExtraInterest : Command
    {
        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the extra interest.
        /// </summary>
        /// <value>The extra interest.</value>
        public int ExtraInterest { get; set; }

        /// <summary>
        ///     Gets or sets the extra principal.
        /// </summary>
        /// <value>The extra principal.</value>
        public int ExtraPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the operation identifier.
        /// </summary>
        /// <value>The operation identifier.</value>
        public Guid OperationId { get; set; }

        /// <summary>
        ///     Gets or sets the order identifier.
        /// </summary>
        /// <value>The order identifier.</value>
        public Guid OrderId { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}