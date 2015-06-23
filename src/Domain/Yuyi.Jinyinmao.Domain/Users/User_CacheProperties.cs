// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  12:25 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-23  2:37 PM
// ***********************************************************************
// <copyright file="User_CacheProperties.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

namespace Yuyi.Jinyinmao.Domain
{
    public partial class User
    {
        /// <summary>
        ///     Gets or sets the crediting settle account amount.
        /// </summary>
        /// <value>The crediting settle account amount.</value>
        private long CreditingSettleAccountAmount { get; set; }

        /// <summary>
        ///     Gets or sets the debiting settle account amount.
        /// </summary>
        /// <value>The debiting settle account amount.</value>
        private long DebitingSettleAccountAmount { get; set; }

        /// <summary>
        ///     Gets or sets the investing interest.
        /// </summary>
        /// <value>The investing interest.</value>
        private long InvestingInterest { get; set; }

        /// <summary>
        ///     Gets or sets the principal.
        /// </summary>
        /// <value>The principal.</value>
        private long InvestingPrincipal { get; set; }

        /// <summary>
        ///     Gets or sets the jby accrual amount.
        /// </summary>
        /// <value>The jby accrual amount.</value>
        private long JBYAccrualAmount { get; set; }

        /// <summary>
        ///     Gets or sets the jby last interest.
        /// </summary>
        /// <value>The jby last interest.</value>
        private long JBYLastInterest { get; set; }

        /// <summary>
        ///     Gets or sets the jby total amount.
        /// </summary>
        /// <value>The jby total amount.</value>
        private long JBYTotalAmount { get; set; }

        /// <summary>
        ///     Gets or sets the jby total interest.
        /// </summary>
        /// <value>The jby total interest.</value>
        private long JBYTotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the jby total pricipal.
        /// </summary>
        /// <value>The jby total pricipal.</value>
        private long JBYTotalPricipal { get; set; }

        /// <summary>
        ///     Gets or sets the jby withdrawalable amount.
        /// </summary>
        /// <value>The jby withdrawalable amount.</value>
        private long JBYWithdrawalableAmount { get; set; }

        /// <summary>
        ///     Gets or sets the month withdrawal count.
        /// </summary>
        /// <value>The month withdrawal count.</value>
        private int MonthWithdrawalCount { get; set; }

        /// <summary>
        ///     Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        private int PasswordErrorCount { get; set; }

        /// <summary>
        ///     Gets or sets the payment password error count.
        /// </summary>
        /// <value>The payment password error count.</value>
        private int PaymentPasswordErrorCount { get; set; }

        /// <summary>
        ///     Gets or sets the settlement account balance.
        /// </summary>
        /// <value>The settlement account balance.</value>
        private long SettleAccountBalance { get; set; }

        /// <summary>
        ///     Gets or sets the today jby withdrawal amount.
        /// </summary>
        /// <value>The today jby withdrawal amount.</value>
        private long TodayJBYWithdrawalAmount { get; set; }

        /// <summary>
        ///     Gets or sets the today withdrawal count.
        /// </summary>
        /// <value>The today withdrawal count.</value>
        private int TodayWithdrawalCount { get; set; }

        /// <summary>
        ///     Gets or sets the total interest.
        /// </summary>
        /// <value>The total interest.</value>
        private long TotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the total principal.
        /// </summary>
        /// <value>The total principal.</value>
        private long TotalPrincipal { get; set; }
    }
}