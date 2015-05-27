// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  6:09 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  11:47 AM
// ***********************************************************************
// <copyright file="TranscationInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
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
    ///     TranscationInfoResponse.
    /// </summary>
    public class SettleAccountTranscationInfoResponse : IResponse
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
        ///     相关的订单唯一标识, 如非必须，为一串0字符
        /// </summary>
        [Required, JsonProperty("orderIdentifier")]
        public string OrderIdentifier { get; set; }

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

    internal static class SettleAccountTranscationInfoEx
    {
        internal static SettleAccountTranscationInfoResponse ToResponse(this SettleAccountTranscationInfo info)
        {
            SettleAccountTranscationInfoResponse response = new SettleAccountTranscationInfoResponse
            {
                Amount = info.Amount,
                BankCardNo = info.BankCardNo,
                ChannelCode = info.ChannelCode,
                OrderIdentifier = info.OrderId.ToGuidString(),
                ResultCode = info.ResultCode,
                ResultTime = info.ResultTime.GetValueOrDefault(),
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