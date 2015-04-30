// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  1:19 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  9:44 PM
// ***********************************************************************
// <copyright file="BankCard.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     BankCard.
    /// </summary>
    public class BankCard
    {
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
        ///     银行所属城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is default.
        /// </summary>
        /// <value><c>true</c> if this instance is default; otherwise, <c>false</c>.</value>
        public bool IsDefault { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="BankCard" /> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [verified by yilian].
        /// </summary>
        /// <value><c>true</c> if [verified by yilian]; otherwise, <c>false</c>.</value>
        public bool VerifiedByYilian { get; set; }

        /// <summary>
        ///     Gets or sets the verified time.
        /// </summary>
        /// <value>The verified time.</value>
        public DateTime? VerifiedTime { get; set; }

        /// <summary>
        /// 可提现额度
        /// </summary>
        public int WithdrawAmount { get; set; }
    }
}
