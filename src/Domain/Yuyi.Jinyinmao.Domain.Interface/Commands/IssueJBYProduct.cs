// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : IssueJBYProduct.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-17  0:26
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
        ///     第一份协议内容，一般为自动交易授权委托书内容
        /// </summary>
        public string Agreement1 { get; set; }

        /// <summary>
        ///     第二份协议内容，一般为金包银投资协议内容
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
        ///     发行事件，北京时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        ///     产品类别
        /// </summary>
        public long ProductCategory { get; set; }

        /// <summary>
        ///     产品Id
        /// </summary>
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