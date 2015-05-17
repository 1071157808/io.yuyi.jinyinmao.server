// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  7:14 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  1:39 AM
// ***********************************************************************
// <copyright file="ProductInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.Lib;
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
        ///     停售时间（北京时间）
        /// </summary>
        [Required, JsonProperty("endSellTime")]
        public DateTime EndSellTime { get; set; }

        /// <summary>
        ///     募集总金额，以“分”为单位
        /// </summary>
        [Required, JsonProperty("financingSumAmount")]
        public long FinancingSumAmount { get; set; }

        /// <summary>
        ///     额外内容，请参考其他文档
        /// </summary>
        [Required, JsonProperty("info")]
        public Dictionary<string, object> Info { get; set; }

        /// <summary>
        ///     发行编号，即期数，可以重复
        /// </summary>
        [Required, JsonProperty("issueNo")]
        public int IssueNo { get; set; }

        /// <summary>
        ///     发行时间，即上线时间
        /// </summary>
        [Required, JsonProperty("issueTime")]
        public DateTime IssueTime { get; set; }

        /// <summary>
        ///     已售金额，以“万分之一”为单位
        /// </summary>
        [Required, JsonProperty("paidAmount")]
        public long PaidAmount { get; set; }

        /// <summary>
        ///     质押物编号，可以认为是票号、合同号等相关文件的编号
        /// </summary>
        [Required, JsonProperty("pledgeNo")]
        public string PledgeNo { get; set; }

        /// <summary>
        ///     产品类别，详细分类参考文档
        /// </summary>
        [Required, JsonProperty("productCategory")]
        public long ProductCategory { get; set; }

        /// <summary>
        ///     产品唯一标识，应该为32位的guid形式的字符串
        /// </summary>
        [Required, JsonProperty("productIdentifier")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        [Required, JsonProperty("productName")]
        public string ProductName { get; set; }

        /// <summary>
        ///     产品编号，产品的编号，区别于IssueNo
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
        [Required, JsonProperty("repaidTime")]
        public DateTime RepaidTime { get; set; }

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
        [Required, JsonProperty("soldOutTime")]
        public DateTime SoldOutTime { get; set; }

        /// <summary>
        ///     指定日期起息
        /// </summary>
        [Required, JsonProperty("specifyValueDate")]
        public bool SpecifyValueDate { get; set; }

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
        [Required, JsonProperty("valueDate")]
        public DateTime ValueDate { get; set; }

        /// <summary>
        ///     先判断是否是指定日期起息；为0，则为购买当日起息，为n，则为T+n日起息；为-n，则为T-n日起息
        /// </summary>
        [Required, JsonProperty("valueDateMode")]
        public int ValueDateMode { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        [Required, JsonProperty("yield")]
        public int Yield { get; set; }
    }

    internal static class RegularProductInfoEx
    {
        internal static RegularProductInfoResponse ToResponse(this RegularProductInfo info)
        {
            return new RegularProductInfoResponse
            {
                EndSellTime = info.EndSellTime,
                FinancingSumAmount = info.FinancingSumAmount,
                Info = info.Info,
                IssueNo = info.IssueNo,
                IssueTime = info.IssueTime,
                PaidAmount = info.PaidAmount,
                PledgeNo = info.PledgeNo,
                ProductCategory = info.ProductCategory,
                ProductIdentifier = info.ProductId.ToGuidString(),
                ProductName = info.ProductName,
                ProductNo = info.ProductNo,
                Repaid = info.Repaid,
                RepaidTime = info.RepaidTime.GetValueOrDefault(),
                RepaymentDeadline = info.RepaymentDeadline,
                SettleDate = info.SettleDate,
                SoldOut = info.SoldOut,
                SoldOutTime = info.SoldOutTime.GetValueOrDefault(),
                SpecifyValueDate = !info.ValueDateMode.HasValue,
                StartSellTime = info.StartSellTime,
                UnitPrice = info.UnitPrice,
                ValueDate = info.ValueDate.GetValueOrDefault(),
                ValueDateMode = info.ValueDateMode.GetValueOrDefault(),
                Yield = info.Yield
            };
        }
    }
}