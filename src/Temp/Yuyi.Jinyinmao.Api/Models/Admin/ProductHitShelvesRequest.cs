// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  10:18 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  10:49 AM
// ***********************************************************************
// <copyright file="ProductHitShelvesRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     ProductHitShelvesRequest.
    /// </summary>
    public class ProductHitShelvesRequest : IRequest
    {
        /// <summary>
        ///     银行名称
        /// </summary>
        [JsonProperty(PropertyName = "bankName")]
        public string BankName { get; set; }

        /// <summary>
        ///     委托协议内容
        /// </summary>
        [Required, JsonProperty(PropertyName = "consignmentAgreement")]
        public string ConsignmentAgreement { get; set; }

        /// <summary>
        ///     汇票付款人
        /// </summary>
        [MaxLength(20000), JsonProperty(PropertyName = "drawee")]
        public string Drawee { get; set; }

        /// <summary>
        ///     汇票付款人信息
        /// </summary>
        [JsonProperty(PropertyName = "draweeInfo")]
        public string DraweeInfo { get; set; }

        /// <summary>
        ///     背书图片链接
        /// </summary>
        [Required, JsonProperty(PropertyName = "endorseImageLink")]
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     停售时间
        /// </summary>
        [Required, JsonProperty(PropertyName = "endSellTime")]
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     融资企业信息
        /// </summary>
        [JsonProperty(PropertyName = "enterpriseInfo")]
        public string EnterpriseInfo { get; set; }

        /// <summary>
        ///     融资企业的营业执照
        /// </summary>
        [JsonProperty(PropertyName = "enterpriseLicense")]
        public string EnterpriseLicense { get; set; }

        /// <summary>
        ///     融资企业名称
        /// </summary>
        [Required, JsonProperty(PropertyName = "enterpriseName")]
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     最大融资额度，以“分”为单位
        /// </summary>
        [Required, Range(1, int.MaxValue), JsonProperty(PropertyName = "financingSumCount")]
        public int FinancingSumCount { get; set; }

        /// <summary>
        ///     担保方
        /// </summary>
        [JsonProperty(PropertyName = "guarantor")]
        public string Guarantor { get; set; }

        /// <summary>
        ///     担保方信息
        /// </summary>
        [JsonProperty(PropertyName = "guarantorInfo")]
        public string GuarantorInfo { get; set; }

        /// <summary>
        ///     担保方式
        /// </summary>
        [JsonProperty(PropertyName = "guarantorMode")]
        public string GuarantorMode { get; set; }

        /// <summary>
        ///     发行期数，可以重复，必须大于0
        /// </summary>
        [Required, Range(0, 1000000000), JsonProperty(PropertyName = "issueNo")]
        public int IssueNo { get; set; }

        /// <summary>
        ///     理财周期，主要用于显示
        /// </summary>
        [Required, Range(1, 36500), JsonProperty(PropertyName = "period")]
        public int Period { get; set; }

        /// <summary>
        ///     抵押协议内容
        /// </summary>
        [Required, JsonProperty(PropertyName = "pledgeAgreement")]
        public string PledgeAgreement { get; set; }

        /// <summary>
        ///     抵押物编号
        /// </summary>
        [Required, MaxLength(40), JsonProperty(PropertyName = "pledgeNo")]
        public string PledgeNo { get; set; }

        /// <summary>
        ///     产品分类
        /// </summary>
        [JsonProperty(PropertyName = "productCategory")]
        public long ProductCategory { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        [Required, MaxLength(50), JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        [Required, MaxLength(40), JsonProperty(PropertyName = "productNo")]
        public string ProductNo { get; set; }

        /// <summary>
        ///     最迟还款日
        /// </summary>
        [Required, JsonProperty(PropertyName = "repaymentDeadline")]
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     结息日
        /// </summary>
        [Required, JsonProperty(PropertyName = "settleDate")]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        [Required, JsonProperty(PropertyName = "startSellTime")]
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     单价，以“分”为单位，10000即每份100元
        /// </summary>
        [Range(1, 2000000000), JsonProperty(PropertyName = "unitPrice")]
        public int UnitPrice { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        [JsonProperty(PropertyName = "usage")]
        public string Usage { get; set; }

        /// <summary>
        ///     指定的起息日，可以为空
        /// </summary>
        [JsonProperty(PropertyName = "valueDate")]
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     起息方式
        /// </summary>
        [JsonProperty(PropertyName = "valueDateMode")]
        public int? ValueDateMode { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        [Required, Range(0, 100), JsonProperty(PropertyName = "yield")]
        public decimal Yield { get; set; }
    }
}
