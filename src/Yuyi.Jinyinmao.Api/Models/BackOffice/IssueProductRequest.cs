// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-23  2:47 PM
// ***********************************************************************
// <copyright file="IssueProductRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     IssueProductRequest.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class IssueProductRequest : IRequest
    {
        /// <summary>
        ///     第一份协议内容，一般为委托协议内容
        /// </summary>
        [Required, JsonProperty("agreement1")]
        public string Agreement1 { get; set; }

        /// <summary>
        ///     第二份协议内容，一般为抵押协议内容
        /// </summary>
        [Required, JsonProperty("agreement2")]
        public string Agreement2 { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        [JsonProperty("bankName")]
        public string BankName { get; set; }

        /// <summary>
        ///     汇票付款人
        /// </summary>
        [MaxLength(20000), JsonProperty("drawee")]
        public string Drawee { get; set; }

        /// <summary>
        ///     汇票付款人信息
        /// </summary>
        [JsonProperty("draweeInfo")]
        public string DraweeInfo { get; set; }

        /// <summary>
        ///     背书图片链接
        /// </summary>
        [Required, RegularExpression("^(https?|ftp)://[^\\s/$.?#].[^\\s]*$"), JsonProperty("endorseImageLink")]
        public string EndorseImageLink { get; set; }

        /// <summary>
        ///     停售时间
        /// </summary>
        [Required, JsonProperty("endSellTime")]
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     融资企业信息
        /// </summary>
        [JsonProperty("enterpriseInfo")]
        public string EnterpriseInfo { get; set; }

        /// <summary>
        ///     融资企业的营业执照
        /// </summary>
        [JsonProperty("enterpriseLicense")]
        public string EnterpriseLicense { get; set; }

        /// <summary>
        ///     融资企业名称
        /// </summary>
        [Required, JsonProperty("enterpriseName")]
        public string EnterpriseName { get; set; }

        /// <summary>
        ///     最大融资额度，以“分”为单位
        /// </summary>
        [Required, Range(1, long.MaxValue), JsonProperty("financingSumAmount")]
        public long FinancingSumAmount { get; set; }

        /// <summary>
        ///     发行期数，可以重复，必须大于0
        /// </summary>
        [Required, Range(0, 1000000000), JsonProperty("issueNo")]
        public int IssueNo { get; set; }

        /// <summary>
        ///     理财周期，主要用于显示
        /// </summary>
        [Required, Range(1, 36500), JsonProperty("period")]
        public int Period { get; set; }

        /// <summary>
        ///     抵押物编号
        /// </summary>
        [Required, MaxLength(40), JsonProperty("pledgeNo")]
        public string PledgeNo { get; set; }

        /// <summary>
        ///     产品分类
        /// </summary>
        [Required, JsonProperty("productCategory")]
        public long ProductCategory { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        [Required, MaxLength(50), JsonProperty("productName")]
        public string ProductName { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        [Required, MaxLength(40), JsonProperty("productNo")]
        public string ProductNo { get; set; }

        /// <summary>
        ///     最迟还款日
        /// </summary>
        [Required, JsonProperty("repaymentDeadline")]
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     风控方
        /// </summary>
        [JsonProperty("riskManagement")]
        public string RiskManagement { get; set; }

        /// <summary>
        ///     风控方信息
        /// </summary>
        [JsonProperty("riskManagementInfo")]
        public string RiskManagementInfo { get; set; }

        /// <summary>
        ///     风控方式
        /// </summary>
        [JsonProperty("riskManagementMode")]
        public string RiskManagementMode { get; set; }

        /// <summary>
        ///     结息日
        /// </summary>
        [Required, JsonProperty("settleDate")]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        [Required, JsonProperty("startSellTime")]
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     单价，以“分”为单位，10000即每份100元
        /// </summary>
        [Required, Range(1, int.MaxValue), JsonProperty("unitPrice")]
        public int UnitPrice { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        [JsonProperty("usage")]
        public string Usage { get; set; }

        /// <summary>
        ///     指定的起息日，可以为空
        /// </summary>
        [JsonProperty("valueDate")]
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     起息方式
        /// </summary>
        [JsonProperty("valueDateMode")]
        public int? ValueDateMode { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        [Required, Range(0, 10000), JsonProperty("yield")]
        public int Yield { get; set; }
    }
}