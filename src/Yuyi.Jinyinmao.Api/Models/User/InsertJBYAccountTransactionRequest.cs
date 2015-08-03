// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : InsertJBYAccountTransactionRequest.cs
// Created          : 2015-08-03  11:33 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-03  11:36 AM
// ***********************************************************************
// <copyright file="InsertJBYAccountTransactionRequest.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     InsertJBYAccountTransactionRequest.
    /// </summary>
    public class InsertJBYAccountTransactionRequest : IRequest
    {
        /// <summary>
        ///     流水金额
        /// </summary>
        [Required, Range(1, 200000000), JsonProperty("amount")]
        public long Amount { get; set; }

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