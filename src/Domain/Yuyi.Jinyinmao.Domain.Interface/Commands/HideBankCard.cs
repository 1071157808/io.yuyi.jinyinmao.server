// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-21  4:23 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-21  4:23 PM
// ***********************************************************************
// <copyright file="HideBankCard.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using Orleans.Concurrency;

namespace Yuyi.Jinyinmao.Domain.Commands
{
    /// <summary>
    ///     HideBankCard.
    /// </summary>
    [Immutable]
    public class HideBankCard : Command
    {
        /// <summary>
        ///     银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }
    }
}