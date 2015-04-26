// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-26  1:14 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-27  12:12 AM
// ***********************************************************************
// <copyright file="AddBankCard.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     AddBankCard.
    /// </summary>
    [Immutable]
    public class AddBankCard : Command
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
        ///     银行所属城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}
