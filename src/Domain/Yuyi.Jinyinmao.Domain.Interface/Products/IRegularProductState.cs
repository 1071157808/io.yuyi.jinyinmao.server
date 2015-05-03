// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  12:31 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  4:52 AM
// ***********************************************************************
// <copyright file="IRegularProductState.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     Interface IRegularProductState
    /// </summary>
    public interface IRegularProductState : IEntityState
    {
        /// <summary>
        ///     第一份协议内容，一般为委托协议内容
        /// </summary>
        string Agreement1 { get; set; }

        /// <summary>
        ///     第二份协议内容，一般为抵押协议内容
        /// </summary>
        string Agreement2 { get; set; }

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        string Args { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        string BankName { get; set; }

        /// <summary>
        ///     汇票付款人
        /// </summary>
        string Drawee { get; set; }

        /// <summary>
        ///     汇票付款人信息
        /// </summary>
        string DraweeInfo { get; set; }

        /// <summary>
        ///     背书图片链接
        /// </summary>
        string EndorseImageLink { get; set; }

        /// <summary>
        ///     停售时间
        /// </summary>
        DateTime EndSellTime { get; set; }

        /// <summary>
        ///     融资企业信息
        /// </summary>
        string EnterpriseInfo { get; set; }

        /// <summary>
        ///     融资企业的营业执照
        /// </summary>
        string EnterpriseLicense { get; set; }

        /// <summary>
        ///     融资企业名称
        /// </summary>
        string EnterpriseName { get; set; }

        /// <summary>
        ///     最大融资额度，以“分”为单位
        /// </summary>
        int FinancingSumAmount { get; set; }

        /// <summary>
        ///     发行期数，可以重复，必须大于0
        /// </summary>
        int IssueNo { get; set; }

        /// <summary>
        ///     上线时间
        /// </summary>
        DateTime IssueTime { get; set; }

        /// <summary>
        ///     订单
        /// </summary>
        List<Order> Orders { get; set; }

        /// <summary>
        ///     理财周期，主要用于显示
        /// </summary>
        int Period { get; set; }

        /// <summary>
        ///     抵押物编号
        /// </summary>
        string PledgeNo { get; set; }

        /// <summary>
        ///     产品分类
        /// </summary>
        long ProductCategory { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        string ProductName { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        string ProductNo { get; set; }

        /// <summary>
        ///     是否已经还款
        /// </summary>
        bool Repaid { get; set; }

        /// <summary>
        ///     实际还款时间
        /// </summary>
        DateTime? RepaidTime { get; set; }

        /// <summary>
        ///     最迟还款日
        /// </summary>
        DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     风控方
        /// </summary>
        string RiskManagement { get; set; }

        /// <summary>
        ///     风控方信息
        /// </summary>
        string RiskManagementInfo { get; set; }

        /// <summary>
        ///     风控措施
        /// </summary>
        string RiskManagementMode { get; set; }

        /// <summary>
        ///     结息日
        /// </summary>
        DateTime SettleDate { get; set; }

        /// <summary>
        ///     是否售罄
        /// </summary>
        bool SoldOut { get; set; }

        /// <summary>
        ///     实际售罄时间
        /// </summary>
        DateTime? SoldOutTime { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        DateTime StartSellTime { get; set; }

        /// <summary>
        ///     单价，以“分”为单位，10000即每份100元
        /// </summary>
        int UnitPrice { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        string Usage { get; set; }

        /// <summary>
        ///     指定的起息日，可以为空
        /// </summary>
        DateTime? ValueDate { get; set; }

        /// <summary>
        ///     起息方式
        /// </summary>
        int? ValueDateMode { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        int Yield { get; set; }
    }
}