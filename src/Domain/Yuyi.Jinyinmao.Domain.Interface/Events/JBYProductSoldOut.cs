// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  4:31 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  11:44 PM
// ***********************************************************************
// <copyright file="JBYProductSoldOut.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;
using Orleans.Concurrency;
using PostSharp.Patterns.Model;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYProductSoldOut.
    /// </summary>
    [Immutable]
    public class JBYProductSoldOut : Event
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
        [Reference]
        public JBYProductInfo ProductInfo { get; set; }

        /// <summary>
        ///     Gets or sets the transactions.
        /// </summary>
        /// <value>The transactions.</value>
        [Reference]
        public List<JBYAccountTransactionInfo> Transactions { get; set; }
    }
}