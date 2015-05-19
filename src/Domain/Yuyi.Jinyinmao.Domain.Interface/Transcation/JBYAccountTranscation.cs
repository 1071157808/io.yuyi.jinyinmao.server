// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-17  4:24 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-17  7:21 PM
// ***********************************************************************
// <copyright file="JBYAccountTranscation.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     JBYAccountTranscation.
    /// </summary>
    public class JBYAccountTranscation
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public int Amount { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Dictionary<string, object> Args { get; set; }

        /// <summary>
        /// Gets or sets the predetermined result date.
        /// </summary>
        /// <value>The predetermined result date.</value>
        public DateTime? PredeterminedResultDate { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }

        /// <summary>
        ///     Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        public DateTime? ResultTime { get; set; }

        /// <summary>
        /// Gets or sets the settle account transcation identifier.
        /// </summary>
        /// <value>The settle account transcation identifier.</value>
        public Guid SettleAccountTranscationId { get; set; }

        /// <summary>
        ///     Gets or sets the trade.
        /// </summary>
        /// <value>The trade.</value>
        public Trade Trade { get; set; }

        /// <summary>
        ///     Gets or sets the trade code.
        /// </summary>
        /// <value>The trade code.</value>
        public int TradeCode { get; set; }

        /// <summary>
        ///     Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The transaction identifier.</value>
        public Guid TransactionId { get; set; }

        /// <summary>
        ///     Gets or sets the transaction time.
        /// </summary>
        /// <value>The transaction time.</value>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}