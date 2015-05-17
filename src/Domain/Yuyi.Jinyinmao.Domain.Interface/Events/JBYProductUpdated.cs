// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  12:29 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  10:16 PM
// ***********************************************************************
// <copyright file="JBYProductUpdated.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYProductUpdated.
    /// </summary>
    [Immutable]
    public class JBYProductUpdated : Event
    {
        /// <summary>
        ///     协议1
        /// </summary>
        public string Agreement1 { get; set; }

        /// <summary>
        ///     协议2
        /// </summary>
        public string Agreement2 { get; set; }

        /// <summary>
        ///     Gets or sets the product information.
        /// </summary>
        /// <value>The product information.</value>
        public JBYProductInfo ProductInfo { get; set; }
    }
}