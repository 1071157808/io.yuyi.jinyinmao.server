// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-09  1:18 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-09  1:21 AM
// ***********************************************************************
// <copyright file="RegularProductSoldOut.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     RegularProductSoldOut.
    /// </summary>
    [Immutable]
    public class RegularProductSoldOut : Event
    {
        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the sold out time.
        /// </summary>
        /// <value>The sold out time.</value>
        public DateTime SoldOutTime { get; set; }

        /// <summary>
        ///     Gets or sets the paid orders.
        /// </summary>
        /// <value>The paid orders.</value>
        public List<Order> PaidOrders { get; set; }

        /// <summary>
        ///     Gets or sets the paid amount.
        /// </summary>
        /// <value>The paid amount.</value>
        public int PaidAmount { get; set; }
    }
}