// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  12:25 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  12:28 PM
// ***********************************************************************
// <copyright file="User_CacheProperties.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Collections.Generic;

namespace Yuyi.Jinyinmao.Domain
{
    public partial class User
    {
        /// <summary>
        ///     Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int PasswordErrorCount { get; set; }

        /// <summary>
        ///     用户所有的银行卡，包括未通过认证的
        /// </summary>
        /// <value>The bank cards.</value>
        private Dictionary<string, BankCard> BankCards { get; set; }

        /// <summary>
        /// Gets or sets the crediting settle account amount.
        /// </summary>
        /// <value>The crediting settle account amount.</value>
        private int CreditingSettleAccountAmount { get; set; }

        /// <summary>
        /// Gets or sets the debiting settle account amount.
        /// </summary>
        /// <value>The debiting settle account amount.</value>
        private int DebitingSettleAccountAmount { get; set; }

        /// <summary>
        ///     Gets or sets the investing interest.
        /// </summary>
        /// <value>The investing interest.</value>
        private int InvestingInterest { get; set; }

        /// <summary>
        ///     Gets or sets the principal.
        /// </summary>
        /// <value>The principal.</value>
        private int InvestingPrincipal { get; set; }

        /// <summary>
        ///     Gets or sets the month withdrawal count.
        /// </summary>
        /// <value>The month withdrawal count.</value>
        private int MonthWithdrawalCount { get; set; }

        /// <summary>
        ///     Gets or sets the payment password error count.
        /// </summary>
        /// <value>The payment password error count.</value>
        private int PaymentPasswordErrorCount { get; set; }

        /// <summary>
        ///     Gets or sets the settlement account balance.
        /// </summary>
        /// <value>The settlement account balance.</value>
        private int SettleAccountBalance { get; set; }

        /// <summary>
        ///     Gets or sets the today withdrawal count.
        /// </summary>
        /// <value>The today withdrawal count.</value>
        private int TodayWithdrawalCount { get; set; }

        /// <summary>
        ///     Gets or sets the total interest.
        /// </summary>
        /// <value>The total interest.</value>
        private int TotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the total principal.
        /// </summary>
        /// <value>The total principal.</value>
        private int TotalPrincipal { get; set; }
    }
}