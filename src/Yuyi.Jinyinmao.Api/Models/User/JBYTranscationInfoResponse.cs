// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  2:07 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  2:20 AM
// ***********************************************************************
// <copyright file="JBYTranscationInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        ///     银行卡号
        /// </summary>
        [Required, JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     渠道号
        /// </summary>
        [Required, JsonProperty("channelCode")]
        public int ChannelCode { get; set; }

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

    internal static partial class TranscationInfoEx
    {
        internal static JBYTranscationInfoResponse ToJBYTranscationInfoResponse(this TranscationInfo info)
        {
            string productIdentifier = string.Empty;
            string productId = info.Info.FirstOrDefault(kv => kv.Key == "ProductId").ToString();
            if (productId.IsNotNullOrEmpty())
            {
                productIdentifier = Guid.Parse(productId).ToGuidString();
            }

            return new JBYTranscationInfoResponse
            {
                Amount = info.Amount,
                BankCardNo = info.BankCardNo,
                ChannelCode = info.ChannelCode,
                ProductIdentifier = productIdentifier,
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime.GetValueOrDefault(),
                Trade = info.Trade,
                TradeCode = info.TradeCode,
                TransactionIdentifier = info.TransactionId.ToGuidString(),
                TransactionTime = info.TransactionTime,
                TransDesc = info.TransDesc
            };
        }
    }
}