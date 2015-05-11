// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  12:27 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  11:49 PM
// ***********************************************************************
// <copyright file="IJBYProductState.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     Interface IJBYProductState
    /// </summary>
    public interface IJBYProductState : IEntityState
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
        Dictionary<string, object> Args { get; set; }

        /// <summary>
        ///     停售时间
        /// </summary>
        DateTime EndSellTime { get; set; }

        /// <summary>
        ///     最大融资额度，以“分”为单位
        /// </summary>
        int FinancingSumAmount { get; set; }

        /// <summary>
        ///     额外的业务数据
        /// </summary>
        Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     发行期数，可以重复，必须大于0
        /// </summary>
        int IssueNo { get; set; }

        /// <summary>
        ///     上线时间
        /// </summary>
        DateTime IssueTime { get; set; }

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
        ///     JBY交易流水
        /// </summary>
        List<TranscationInfo> Transcations { get; set; }

        /// <summary>
        ///     单价，以“分”为单位，10000即每份100元
        /// </summary>
        int UnitPrice { get; set; }

        /// <summary>
        ///     起息方式
        /// </summary>
        int ValueDateMode { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        int Yield { get; set; }
    }
}