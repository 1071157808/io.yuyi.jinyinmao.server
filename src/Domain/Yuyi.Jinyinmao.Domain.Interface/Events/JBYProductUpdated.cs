// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYProductUpdated.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  1:54
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