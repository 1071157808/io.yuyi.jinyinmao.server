// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-18  12:11 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-22  6:54 PM
// ***********************************************************************
// <copyright file="RegularProductRepaid.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     RegularProductRepaid.
    /// </summary>
    [Immutable]
    public class RegularProductRepaid : Event
    {
        /// <summary>
        ///     协议1
        /// </summary>
        /// <value>The agreement1.</value>
        public string Agreement1 { get; set; }

        /// <summary>
        ///     协议2
        /// </summary>
        /// <value>The agreement2.</value>
        public string Agreement2 { get; set; }

        /// <summary>
        ///     Gets or sets the paid amount.
        /// </summary>
        /// <value>The paid amount.</value>
        public long PaidAmount { get; set; }

        /// <summary>
        ///     Gets or sets the paid orders.
        /// </summary>
        /// <value>The paid orders.</value>
        public List<string> PaidOrders { get; set; }

        /// <summary>
        ///     Gets or sets the product information.
        /// </summary>
        /// <value>The product information.</value>
        public RegularProductInfo ProductInfo { get; set; }
    }
}