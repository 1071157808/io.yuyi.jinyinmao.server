// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-11  12:47 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-11  1:18 AM
// ***********************************************************************
// <copyright file="JBYInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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

namespace Yuyi.Jinyinmao.Api.Models.Product
{
    /// <summary>
    ///     JBYInfoResponse.
    /// </summary>
    public class JBYInfoResponse : IResponse
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
        public int FinancingSumAmount { get; set; }

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
        public int PaidAmount { get; set; }

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

    internal static class JBYProductInfoEx
    {
        internal static JBYInfoResponse ToResponse(this JBYProductInfo info)
        {
            return new JBYInfoResponse
            {
                EndSellTime = info.EndSellTime,
                FinancingSumAmount = info.FinancingSumAmount,
                Info = info.Info,
                IssueNo = info.IssueNo,
                IssueTime = info.IssueTime,
                PaidAmount = info.PaidAmount,
                ProductCategory = info.ProductCategory,
                ProductIdentifier = info.ProductId.ToGuidString(),
                ProductName = info.ProductName,
                ProductNo = info.ProductNo,
                SoldOut = info.SoldOut,
                SoldOutTime = info.SoldOutTime.GetValueOrDefault(),
                StartSellTime = info.StartSellTime,
                UnitPrice = info.UnitPrice,
                ValueDateMode = info.ValueDateMode,
                Yield = info.Yield
            };
        }
    }
}