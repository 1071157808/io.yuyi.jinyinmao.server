// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-03  4:46 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  2:21 AM
// ***********************************************************************
// <copyright file="DepositFromYilianRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     DepositFromYilianRequest.
    /// </summary>
    public class DepositFromYilianRequest : IRequest
    {
        /// <summary>
        ///     支付金额，以“分”为单位
        /// </summary>
        /// <value>The amount.</value>
        [Required, Range(100, 30000000), JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        ///     银行卡号，15到19位
        /// </summary>
        [Required, StringLength(19, MinimumLength = 15), JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }
    }
}