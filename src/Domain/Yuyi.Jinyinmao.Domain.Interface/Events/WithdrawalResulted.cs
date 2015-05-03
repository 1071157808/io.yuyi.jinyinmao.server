// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  2:05 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  2:27 AM
// ***********************************************************************
// <copyright file="WithdrawalResulted.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain.Events
{
    /// <summary>
    ///     WithdrawalResulted.
    /// </summary>
    public class WithdrawalResulted : Event
    {
        /// <summary>
        ///     取现金额，以“分”为单位
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        ///     用户手机号
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        ///     银行卡所在城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        ///     证件号类型
        /// </summary>
        public Credential Credential { get; set; }

        /// <summary>
        ///     证件号码
        /// </summary>
        public string CredentialNo { get; set; }

        /// <summary>
        ///     用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="DepositFromYilianResulted" /> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }

        /// <summary>
        ///     Gets or sets the result code.
        /// </summary>
        /// <value>The result code.</value>
        public int ResultCode { get; set; }

        /// <summary>
        ///     Gets or sets the result time.
        /// </summary>
        /// <value>The result time.</value>
        public DateTime ResultTime { get; set; }

        /// <summary>
        ///     操作完成后的账户余额
        /// </summary>
        public int SettleAccountBalance { get; set; }

        /// <summary>
        ///     Gets or sets the trade.
        /// </summary>
        /// <value>The trade.</value>
        public Trade Trade { get; set; }

        /// <summary>
        ///     Gets or sets the transcation identifier.
        /// </summary>
        /// <value>The transcation identifier.</value>
        public Guid TranscationId { get; set; }

        /// <summary>
        ///     Gets or sets the transcation time.
        /// </summary>
        /// <value>The transcation time.</value>
        public DateTime TranscationTime { get; set; }

        /// <summary>
        ///     Gets or sets the trans desc.
        /// </summary>
        /// <value>The trans desc.</value>
        public string TransDesc { get; set; }

        /// <summary>
        ///     用户唯一标示符
        /// </summary>
        public Guid UserId { get; set; }
    }
}