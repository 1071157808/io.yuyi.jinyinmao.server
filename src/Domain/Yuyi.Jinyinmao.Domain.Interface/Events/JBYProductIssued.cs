// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  2:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  2:43 AM
// ***********************************************************************
// <copyright file="JBYProductIssued.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     JBYProductIssued.
    /// </summary>
    [Immutable]
    public class JBYProductIssued : Event
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
        ///     停售时间
        /// </summary>
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     最大融资额度，以“分”为单位
        /// </summary>
        public int FinancingSumAmount { get; set; }

        /// <summary>
        ///     发行期数，可以重复，必须大于0
        /// </summary>
        public int IssueNo { get; set; }

        /// <summary>
        ///     发行时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        ///     产品分类
        /// </summary>
        public long ProductCategory { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public Guid ProductId { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     单价，以“分”为单位，10000即每份100元
        /// </summary>
        public int UnitPrice { get; set; }

        /// <summary>
        ///     起息方式
        /// </summary>
        public int ValueDateMode { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        public int Yield { get; set; }
    }
}