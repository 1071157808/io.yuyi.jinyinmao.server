// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  12:25 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  2:43 AM
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
        private int CreditingSettleAccountAmount { get; set; }

        /// <summary>
        ///     Gets or sets the debiting settle account amount.
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
        ///     Gets or sets the jby accrual amount.
        /// </summary>
        /// <value>The jby accrual amount.</value>
        private int JBYAccrualAmount { get; set; }

        /// <summary>
        ///     Gets or sets the jby total amount.
        /// </summary>
        /// <value>The jby total amount.</value>
        private int JBYTotalAmount { get; set; }

        /// <summary>
        ///     Gets or sets the jby total interest.
        /// </summary>
        /// <value>The jby total interest.</value>
        private int JBYTotalInterest { get; set; }

        /// <summary>
        ///     Gets or sets the jby total pricipal.
        /// </summary>
        /// <value>The jby total pricipal.</value>
        private int JBYTotalPricipal { get; set; }

        /// <summary>
        ///     Gets or sets the jby withdrawalable amount.
        /// </summary>
        /// <value>The jby withdrawalable amount.</value>
        private int JBYWithdrawalableAmount { get; set; }

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
        private int SettleAccountBalance { get; set; }

        /// <summary>
        ///     Gets or sets the today jby withdrawal amount.
        /// </summary>
        /// <value>The today jby withdrawal amount.</value>
        private int TodayJBYWithdrawalAmount { get; set; }

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