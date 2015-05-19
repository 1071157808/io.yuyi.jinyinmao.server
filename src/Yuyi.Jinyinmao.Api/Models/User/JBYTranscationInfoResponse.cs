// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  2:07 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  11:39 AM
// ***********************************************************************
// <copyright file="JBYTranscationInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.Lib;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     JBYTranscationInfoResponse.
    /// </summary>
    public class JBYTranscationInfoResponse : IResponse
    {
        /// <summary>
        ///     金额，以分为单位
        /// </summary>
        [Required, JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        ///     金包银流水预定的结算时间，根据业务情况，该值可能为无意义值
        /// </summary>
        [Required, JsonProperty("predeterminedResultDate")]
        public DateTime PredeterminedResultDate { get; set; }

        /// <summary>
        ///     对应的金包银产品唯一标识
        /// </summary>
        [Required, JsonProperty("productIdentifier")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     交易结果 -1 => 交易失败，0 => 交易中，1 => 交易成功
        /// </summary>
        [Required, JsonProperty("resultCode")]
        public int ResultCode { get; set; }

        /// <summary>
        ///     交易确认时间
        /// </summary>
        [Required, JsonProperty("resultTime")]
        public DateTime ResultTime { get; set; }

        /// <summary>
        ///     对应的钱包流水唯一标识，如果没有相对应的流水，则该值为一串0
        /// </summary>
        [Required, JsonProperty("settleAccountTranscationId")]
        public string SettleAccountTranscationId { get; set; }

        /// <summary>
        ///     交易代码
        /// </summary>
        [Required, JsonProperty("trade")]
        public Trade Trade { get; set; }

        /// <summary>
        ///     交易代码
        /// </summary>
        [Required, JsonProperty("tradeCode")]
        public int TradeCode { get; set; }

        /// <summary>
        ///     交易唯一标识
        /// </summary>
        [Required, JsonProperty("transactionIdentifier")]
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     交易申请时间
        /// </summary>
        [Required, JsonProperty("transactionTime")]
        public DateTime TransactionTime { get; set; }

        /// <summary>
        ///     交易描述
        /// </summary>
        [Required, JsonProperty("transDesc")]
        public string TransDesc { get; set; }
    }

    internal static class JBYAccountTranscationInfoEx
    {
        internal static JBYTranscationInfoResponse ToResponse(this JBYAccountTranscationInfo info)
        {
            JBYTranscationInfoResponse response = new JBYTranscationInfoResponse
            {
                Amount = info.Amount,
                PredeterminedResultDate = info.PredeterminedResultDate.GetValueOrDefault(),
                ProductIdentifier = info.ProductId.ToGuidString(),
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime.GetValueOrDefault(),
                SettleAccountTranscationId = info.SettleAccountTranscationId.ToGuidString(),
                Trade = info.Trade,
                TradeCode = info.TradeCode,
                TransactionIdentifier = info.TransactionId.ToGuidString(),
                TransactionTime = info.TransactionTime,
                TransDesc = info.TransDesc
            };

            return response;
        }
    }
}