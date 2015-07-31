// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : InsertSettleAccountTransactionRequest.cs
// Created          : 2015-07-31  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-31  7:27 PM
// ***********************************************************************
// <copyright file="InsertSettleAccountTransactionRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     InsertSettleAccountTransactionRequest.
    /// </summary>
    public class InsertSettleAccountTransactionRequest : IRequest
    {
        /// <summary>
        ///     流水金额
        /// </summary>
        [Required, Range(1, 200000000), JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        ///     流水对应的银行卡号
        /// </summary>
        [Required, Range(1, 200000000), JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     流水对应的订单唯一标识
        /// </summary>
        [JsonProperty("orderId")]
        public Guid? OrderId { get; set; }

        /// <summary>
        ///     流水交易类型
        /// </summary>
        [Required, JsonProperty("trade")]
        public Trade Trade { get; set; }

        /// <summary>
        ///     流水交易类型
        /// </summary>
        [Required, JsonProperty("tradeCode")]
        public int TradeCode { get; set; }

        /// <summary>
        ///     交易描述
        /// </summary>
        [Required, JsonProperty("transDesc")]
        public string TransDesc { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [Required, JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }
    }
}