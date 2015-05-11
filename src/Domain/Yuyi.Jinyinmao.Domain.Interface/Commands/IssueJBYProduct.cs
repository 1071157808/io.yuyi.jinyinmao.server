// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  1:37 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  1:44 AM
// ***********************************************************************
// <copyright file="IssueJBYProduct.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     IssueJBYProduct.
    /// </summary>
    [Immutable]
    public class IssueJBYProduct : Command
    {
        /// <summary>
        ///     第一份协议内容，一般为委托协议内容
        /// </summary>
        public string Agreement1 { get; set; }

        /// <summary>
        ///     第二份协议内容，一般为抵押协议内容
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
        /// 产品发售当天的JBY体现上限
        /// </summary>
        public long WithdrawalLimit { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        public int Yield { get; set; }
    }
}