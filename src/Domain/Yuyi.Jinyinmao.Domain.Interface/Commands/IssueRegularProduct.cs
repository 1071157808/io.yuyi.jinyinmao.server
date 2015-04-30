// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:17 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  5:54 PM
// ***********************************************************************
// <copyright file="IssueRegularProduct.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     IssueRegularProduct.
    /// </summary>
    [Immutable]
    public class IssueRegularProduct : Command
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
        ///     银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        ///     汇票付款人
        /// </summary>
        public string Drawee { get; set; }

        /// <summary>
        ///     汇票付款人信息
        /// </summary>
        public string DraweeInfo { get; set; }

        /// <summary>
        ///     背书图片链接
        /// </summary>
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     停售时间
        /// </summary>
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     融资企业信息
        /// </summary>
        public string EnterpriseInfo { get; set; }

        /// <summary>
        ///     融资企业的营业执照
        /// </summary>
        public string EnterpriseLicense { get; set; }

        /// <summary>
        ///     融资企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     最大融资额度，以“分”为单位
        /// </summary>
        public int FinancingSumCount { get; set; }

        /// <summary>
        ///     发行期数，可以重复，必须大于0
        /// </summary>
        public int IssueNo { get; set; }

        /// <summary>
        ///     发行时间
        /// </summary>
        public DateTime IssueTime { get; set; }

        /// <summary>
        ///     理财周期，主要用于显示
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        ///     抵押物编号
        /// </summary>
        public string PledgeNo { get; set; }

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
        ///     最迟还款日
        /// </summary>
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     风控方
        /// </summary>
        public string RiskManagement { get; set; }

        /// <summary>
        ///     风控方信息
        /// </summary>
        public string RiskManagementInfo { get; set; }

        /// <summary>
        ///     风控措施
        /// </summary>
        public string RiskManagementMode { get; set; }

        /// <summary>
        ///     结息日
        /// </summary>
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     单价，以“分”为单位，10000即每份100元
        /// </summary>
        public int UnitPrice { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        ///     指定的起息日，可以为空
        /// </summary>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     起息方式
        /// </summary>
        public int? ValueDateMode { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        public decimal Yield { get; set; }
    }
}
