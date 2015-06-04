// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-06-05  1:25 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-05  1:26 AM
// ***********************************************************************
// <copyright file="ExtraInterestAdded.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     ExtraInterestAdded.
    /// </summary>
    [Immutable]
    public class ExtraInterestAdded : Event
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public int Amount { get; set; }

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
        ///     Gets or sets the order information.
        /// </summary>
        /// <value>The order information.</value>
        public OrderInfo OrderInfo { get; set; }
    }
}