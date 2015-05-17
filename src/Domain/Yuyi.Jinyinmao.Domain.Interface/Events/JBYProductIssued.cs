// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  2:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  9:00 PM
// ***********************************************************************
// <copyright file="JBYProductIssued.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using Orleans.Concurrency;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYProductIssued.
    /// </summary>
    [Immutable]
    public class JBYProductIssued : Event
    {
        /// <summary>
        ///     Gets or sets the agreement1.
        /// </summary>
        /// <value>The agreement1.</value>
        public string Agreement1 { get; set; }

        /// <summary>
        ///     Gets or sets the agreement2.
        /// </summary>
        /// <value>The agreement2.</value>
        public string Agreement2 { get; set; }

        /// <summary>
        ///     Gets or sets the product information.
        /// </summary>
        /// <value>The product information.</value>
        public JBYProductInfo ProductInfo { get; set; }
    }
}