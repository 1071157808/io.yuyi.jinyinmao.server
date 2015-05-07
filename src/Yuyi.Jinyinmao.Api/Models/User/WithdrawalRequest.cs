// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  12:29 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  2:47 AM
// ***********************************************************************
// <copyright file="WithdrawalRequest.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     WithdrawalRequest.
    /// </summary>
    public class WithdrawalRequest : IRequest
    {
        /// <summary>
        ///     取现额度，以“分”为单位，1分到100万
        /// </summary>
        /// <value>The amount.</value>
        [Required, Range(1, 100000000), JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        ///     银行卡号，15到19位
        /// </summary>
        [Required, StringLength(19, MinimumLength = 15), JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }
    }
}