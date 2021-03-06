// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : OrderInfoResponse.cs
// Created          : 2015-05-25  4:38 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  6:31 PM
// ***********************************************************************
// <copyright file="OrderInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Moe.AspNet.Models;
using Moe.Lib;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     OrderInfoResponse.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class OrderInfoResponse : IResponse
    {
        /// <summary>
        ///     交易流水唯一标识
        /// </summary>
        [Required, JsonProperty("accountTransactionIdentifier")]
        public string AccountTransactionIdentifier { get; set; }

        /// <summary>
        ///     额外收益，以“分”为单位
        /// </summary>
        [Required, JsonProperty("extraInterest")]
        public long ExtraInterest { get; set; }

        /// <summary>
        ///     额外收益的相关信息
        /// </summary>
        [Required, JsonProperty("extraInterestRecords")]
        public List<ExtraInterestRecord> ExtraInterestRecords { get; set; }

        /// <summary>
        ///     额外收益率，以“万分之一”为单位
        /// </summary>
        [Required, JsonProperty("extraYield")]
        public int ExtraYield { get; set; }

        /// <summary>
        ///     预期收益，以“分”为单位
        /// </summary>
        [Required, JsonProperty("interest")]
        public long Interest { get; set; }

        /// <summary>
        ///     是否已经还款
        /// </summary>
        [Required, JsonProperty("isRepaid")]
        public bool IsRepaid { get; set; }

        /// <summary>
        ///     订单唯一标识
        /// </summary>
        [Required, JsonProperty("orderIdentifier")]
        public string OrderIdentifier { get; set; }

        /// <summary>
        ///     订单编号
        /// </summary>
        [Required, JsonProperty("orderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        ///     下单时间
        /// </summary>
        [Required, JsonProperty("orderTime")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     投资本金
        /// </summary>
        [Required, JsonProperty("principal")]
        public long Principal { get; set; }

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
        ///     产品信息快照
        /// </summary>
        [Required, JsonProperty("productSnapshot")]
        public RegularProductInfoResponse ProductSnapshot { get; set; }

        /// <summary>
        ///     还款时间
        /// </summary>
        [Required, JsonProperty("repaidTime")]
        public DateTime RepaidTime { get; set; }

        /// <summary>
        ///     状态码
        /// </summary>
        [Required, JsonProperty("resultCode")]
        public int ResultCode { get; set; }

        /// <summary>
        ///     订单确认结果时间
        /// </summary>
        [Required, JsonProperty("resultTime")]
        public DateTime ResultTime { get; set; }

        /// <summary>
        ///     结息日期
        /// </summary>
        [Required, JsonProperty("settleDate")]
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     交易描述
        /// </summary>
        [Required, JsonProperty("transDesc")]
        public string TransDesc { get; set; }

        /// <summary>
        ///     起息日期
        /// </summary>
        [Required, JsonProperty("valueDate")]
        public DateTime ValueDate { get; set; }

        /// <summary>
        ///     收益率，以“万分之一”为单位
        /// </summary>
        [Required, JsonProperty("yield")]
        public int Yield { get; set; }
    }

    internal static class OrderInfoEx
    {
        internal static OrderInfoResponse ToResponse(this OrderInfo info)
        {
            return new OrderInfoResponse
            {
                AccountTransactionIdentifier = info.AccountTransactionId.ToGuidString(),
                ExtraInterest = info.ExtraInterest,
                ExtraInterestRecords = info.ExtraInterestRecords,
                ExtraYield = info.ExtraYield,
                Interest = info.Interest,
                IsRepaid = info.IsRepaid,
                OrderIdentifier = info.OrderId.ToGuidString(),
                OrderNo = info.OrderNo,
                OrderTime = info.OrderTime,
                Principal = info.Principal,
                ProductCategory = info.ProductCategory,
                ProductIdentifier = info.ProductId.ToGuidString(),
                ProductSnapshot = info.ProductSnapshot.ToResponse(),
                RepaidTime = info.RepaidTime.GetValueOrDefault(),
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime.GetValueOrDefault(),
                SettleDate = info.SettleDate,
                TransDesc = info.TransDesc,
                ValueDate = info.ValueDate,
                Yield = info.Yield
            };
        }
    }
}