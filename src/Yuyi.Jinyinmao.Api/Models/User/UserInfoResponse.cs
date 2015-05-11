// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-27  11:32 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-28  10:16 AM
// ***********************************************************************
// <copyright file="UserInfoResponse.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using Moe.AspNet.Models;
using Moe.Lib;
using Newtonsoft.Json;
using Yuyi.Jinyinmao.Domain.Dtos;

namespace Yuyi.Jinyinmao.Api.Models
{
    /// <summary>
    ///     UserInfoResponse.
    /// </summary>
    public class UserInfoResponse : IResponse
    {
        /// <summary>
        ///     账户余额，以“分”为单位
        /// </summary>
        [Required, JsonProperty("balance")]
        public int Balance { get; set; }

        /// <summary>
        ///     默认银行卡号
        /// </summary>
        [Required, JsonProperty("bankCardNo")]
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行卡数量
        /// </summary>
        [Required, JsonProperty("bankCardsCount")]
        public int BankCardsCount { get; set; }

        /// <summary>
        ///     默认银行卡银行名称
        /// </summary>
        [Required, JsonProperty("bankName")]
        public string BankName { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        [Required, JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     证件类型。0 => 身份证， 1 => 护照，2 => 台湾， 3=> 军官证
        /// </summary>
        [Required, JsonProperty("credential")]
        public int Credential { get; set; }

        /// <summary>
        ///     证件号码
        /// </summary>
        [Required, JsonProperty("credentialNo")]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     在途的出项金额，以“分”为单位
        /// </summary>
        [Required, JsonProperty("crediting")]
        public int Crediting { get; set; }

        /// <summary>
        ///     在途的进项金额，以“分”为单位
        /// </summary>
        [Required, JsonProperty("debiting")]
        public int Debiting { get; set; }

        /// <summary>
        ///     是否设定了支付密码
        /// </summary>
        [Required, JsonProperty("hasSetPaymentPassword")]
        public bool HasSetPaymentPassword { get; set; }

        /// <summary>
        ///     预期收益
        /// </summary>
        [Required, JsonProperty("investingInterest")]
        public int InvestingInterest { get; set; }

        /// <summary>
        ///     在投资金
        /// </summary>
        [Required, JsonProperty("investingPrincipal")]
        public int InvestingPrincipal { get; set; }

        /// <summary>
        /// 当月取款次数
        /// </summary>
        [Required, JsonProperty("monthWithdrawalCount")]
        public int MonthWithdrawalCount { get; set; }

        /// <summary>
        /// 登录密码错误次数
        /// </summary>
        [Required, JsonProperty("passwordErrorCount")]
        public int PasswordErrorCount { get; set; }

        /// <summary>
        ///     用户真实姓名
        /// </summary>
        [Required, JsonProperty("realName")]
        public string RealName { get; set; }

        /// <summary>
        ///     用户注册时间
        /// </summary>
        [Required, JsonProperty("registerTime")]
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 当天取款次数
        /// </summary>
        [Required, JsonProperty("todayWithdrawalCount")]
        public int TodayWithdrawalCount { get; set; }

        /// <summary>
        ///     总收益
        /// </summary>
        [Required, JsonProperty("totalInterest")]
        public int TotalInterest { get; set; }

        /// <summary>
        ///     总本金
        /// </summary>
        [Required, JsonProperty("totalPrincipal")]
        public int TotalPrincipal { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        [Required, JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     用户是否通过实名认证
        /// </summary>
        [Required, JsonProperty("verified")]
        public bool Verified { get; set; }
    }

    internal static partial class UserInfoEx
    {
        internal static UserInfoResponse ToResponse(this UserInfo info)
        {
            return new UserInfoResponse
            {
                Balance = info.Balance,
                BankCardNo = info.BankCardNo,
                BankCardsCount = info.BankCardsCount,
                BankName = info.BankName,
                Cellphone = info.Cellphone,
                Credential = (int)info.Credential,
                CredentialNo = info.CredentialNo,
                Crediting = info.Crediting,
                Debiting = info.Debiting,
                HasSetPaymentPassword = info.HasSetPaymentPassword,
                InvestingInterest = info.InvestingInterest,
                InvestingPrincipal = info.InvestingPrincipal,
                MonthWithdrawalCount = info.MonthWithdrawalCount,
                PasswordErrorCount = info.PasswordErrorCount,
                RealName = info.RealName,
                RegisterTime = info.RegisterTime,
                TodayWithdrawalCount = info.TodayWithdrawalCount,
                TotalInterest = info.TotalInterest,
                TotalPrincipal = info.TotalPrincipal,
                UserIdentifier = info.UserId.ToGuidString(),
                Verified = info.Verified
            };
        }
    }
}