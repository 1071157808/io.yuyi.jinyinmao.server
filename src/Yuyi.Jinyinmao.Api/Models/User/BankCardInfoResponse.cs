// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-28  1:05 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-10  11:55 AM
// ***********************************************************************
// <copyright file="BankCardInfoResponse.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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
        ///     银行卡关联的手机号
        /// </summary>
        [Required, JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     开户地，格式应该为  **|**
        /// </summary>
        [Required, JsonProperty("cityName")]
        public string CityName { get; set; }

        /// <summary>
        ///     是否已经认证
        /// </summary>
        [Required, JsonProperty("verified")]
        public bool Verified { get; set; }

        /// <summary>
        ///     是否可以用于易联充值
        /// </summary>
        [Required, JsonProperty("verifiedByYilian")]
        public bool VerifiedByYilian { get; set; }

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
        internal static BankCardInfoResponse ToResponse(this BankCardInfo info) => new BankCardInfoResponse
        {
            BankCardNo = info.BankCardNo,
            BankName = info.BankName,
            Cellphone = info.Cellphone,
            CityName = info.CityName,
            Verified = info.Verified,
            VerifiedByYilian = info.VerifiedByYilian,
            VerifiedTime = info.VerifiedTime,
            WithdrawAmount = info.WithdrawAmount
        };
    }
}