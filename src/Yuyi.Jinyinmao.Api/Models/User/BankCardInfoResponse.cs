// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  1:02 PM
// ***********************************************************************
// <copyright file="BankCardInfoResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     BankCardInfoResponse.
    /// </summary>
    public class BankCardInfoResponse : IResponse
    {
        /// <summary>
        ///     银行卡密码
        /// </summary>
        [Required, JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        [Required, JsonProperty("bankName")]
        public string BankName { get; set; }

        /// <summary>
        ///     是否可以用于易联充值
        /// </summary>
        [Required, JsonProperty("canBeUsedForYilian")]
        public bool CanBeUsedForYilian { get; set; }

        /// <summary>
        ///     开户地，格式应该为  **|**
        /// </summary>
        [Required, JsonProperty("cityName")]
        public string CityName { get; set; }

        /// <summary>
        ///     是否是默认银行卡
        /// </summary>
        [Required, JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        /// <summary>
        ///     是否已经认证
        /// </summary>
        [Required, JsonProperty("verified")]
        public bool Verified { get; set; }

        /// <summary>
        ///     认证时间
        /// </summary>
        [Required, JsonProperty("verifiedTime")]
        public DateTime VerifiedTime { get; set; }

        /// <summary>
        ///     可提现额度，单位为“分”
        /// </summary>
        [Required, JsonProperty("withdrawAmount")]
        public int WithdrawAmount { get; set; }
    }

    internal static class BankCardEx
    {
        internal static BankCardInfoResponse ToResponse(this BankCardInfo info)
        {
            return new BankCardInfoResponse
            {
                BankCardNo = info.BankCardNo,
                BankName = info.BankName,
                CanBeUsedForYilian = info.CanBeUsedForYilian,
                CityName = info.CityName,
                IsDefault = info.IsDefault,
                Verified = info.Verified,
                VerifiedTime = info.VerifiedTime,
                WithdrawAmount = info.WithdrawAmount
            };
        }
    }
}