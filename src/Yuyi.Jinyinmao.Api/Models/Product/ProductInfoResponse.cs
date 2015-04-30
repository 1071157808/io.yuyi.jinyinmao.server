// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  7:14 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-30  12:44 AM
// ***********************************************************************
// <copyright file="ProductInfoResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     RegularProductInfoResponse.
    /// </summary>
    public class RegularProductInfoResponse : IResponse
    {
        /// <summary>
        ///     停售时间
        /// </summary>
        [Required, JsonProperty("endSellTime")]
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     募集总份数
        /// </summary>
        [Required, JsonProperty("financingSumCount")]
        public int FinancingSumCount { get; set; }

        /// <summary>
        ///     额外内容，请参考其他文档
        /// </summary>
        [Required, JsonProperty("info")]
        public string Info { get; set; }

        /// <summary>
        ///     发行编号
        /// </summary>
        [Required, JsonProperty("issueNo")]
        public int IssueNo { get; set; }

        /// <summary>
        ///     发行时间
        /// </summary>
        [Required, JsonProperty("issueTime")]
        public DateTime IssueTime { get; set; }

        /// <summary>
        ///     质押物编号
        /// </summary>
        [Required, JsonProperty("pledgeNo")]
        public string PledgeNo { get; set; }

        /// <summary>
        ///     产品类别
        /// </summary>
        [Required, JsonProperty("productCategory")]
        public long ProductCategory { get; set; }

        /// <summary>
        ///     产品唯一标识
        /// </summary>
        [Required, JsonProperty("productIdentifier")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        [Required, JsonProperty("productName")]
        public string ProductName { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        [Required, JsonProperty("productNo")]
        public string ProductNo { get; set; }

        /// <summary>
        ///     产品是否还款
        /// </summary>
        [Required, JsonProperty("repaid")]
        public bool Repaid { get; set; }

        /// <summary>
        ///     还款时间
        /// </summary>
        [JsonProperty("repaidTime")]
        public DateTime? RepaidTime { get; set; }

        /// <summary>
        ///     最迟还款日
        /// </summary>
        [Required, JsonProperty("repaymentDeadline")]
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     结息日
        /// </summary>
        [Required, JsonProperty("settleDate")]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     是否已经售罄
        /// </summary>
        [Required, JsonProperty("soldOut")]
        public bool SoldOut { get; set; }

        /// <summary>
        ///     售罄时间
        /// </summary>
        [JsonProperty("soldOutTime")]
        public DateTime? SoldOutTime { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        [Required, JsonProperty("startSellTime")]
        public DateTime StartSellTime { get; set; }

        /// <summary>
        ///     单价，以“分”为单位
        /// </summary>
        [Required, JsonProperty("unitPrice")]
        public int UnitPrice { get; set; }

        /// <summary>
        ///     起息日
        /// </summary>
        [JsonProperty("valueDate")]
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     起息方式：为null，即为指定时间起息，起息日为起息时间所指定的日期；为0，则为购买当日起息，为n，则为T+n日起息；为-n，则为T-n日起息
        /// </summary>
        [JsonProperty("valueDateMode")]
        public int? ValueDateMode { get; set; }

        /// <summary>
        ///     收益率
        /// </summary>
        [Required, JsonProperty("yield")]
        public decimal Yield { get; set; }
    }

    internal static class RegularProductInfoEx
    {
        internal static RegularProductInfoResponse ToResponse(this RegularProductInfo info)
        {
            return new RegularProductInfoResponse
            {
                EndSellTime = info.EndSellTime,
                FinancingSumCount = info.FinancingSumAmount,
                Info = info.Info,
                IssueNo = info.IssueNo,
                IssueTime = info.IssueTime,
                PledgeNo = info.PledgeNo,
                ProductCategory = info.ProductCategory,
                ProductIdentifier = info.ProductIdentifier,
                ProductName = info.ProductName,
                ProductNo = info.ProductNo,
                Repaid = info.Repaid,
                RepaidTime = info.RepaidTime,
                RepaymentDeadline = info.RepaymentDeadline,
                SettleDate = info.SettleDate,
                SoldOut = info.SoldOut,
                SoldOutTime = info.SoldOutTime,
                StartSellTime = info.StartSellTime,
                UnitPrice = info.UnitPrice,
                ValueDate = info.ValueDate,
                ValueDateMode = info.ValueDateMode,
                Yield = info.Yield
            };
        }
    }
}
