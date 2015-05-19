// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-08  2:57 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  12:00 PM
// ***********************************************************************
// <copyright file="OrderInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.Lib;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models.Order
{
    /// <summary>
    ///     OrderInfoResponse.
    /// </summary>
    public class OrderInfoResponse : IResponse
    {
        /// <summary>
        ///     交易流水唯一标识
        /// </summary>
        [Required, JsonProperty("accountTranscationIdentifier")]
        public string AccountTranscationIdentifier { get; set; }

        /// <summary>
        ///     额外收益，以“分”为单位
        /// </summary>
        [Required, JsonProperty("extraInterest")]
        public int ExtraInterest { get; set; }

        /// <summary>
        ///     额外收益率，以“万分之一”为单位
        /// </summary>
        [Required, JsonProperty("extraYield")]
        public int ExtraYield { get; set; }

        /// <summary>
        ///     预期收益，以“分”为单位
        /// </summary>
        [Required, JsonProperty("interest")]
        public int Interest { get; set; }

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
        public int Principal { get; set; }

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
                AccountTranscationIdentifier = info.AccountTranscationId.ToGuidString(),
                ExtraInterest = info.ExtraInterest,
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