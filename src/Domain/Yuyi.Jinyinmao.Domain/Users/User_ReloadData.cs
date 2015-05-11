// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  12:19 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-07  3:11 PM
// ***********************************************************************
// <copyright file="User_ReloadData.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Moe.Lib;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain
{
    /// <summary>
    ///     User.
    /// </summary>
    public partial class User
    {
        /// <summary>
        ///     Reloads the bank cards data.
        /// </summary>
        private void ReloadBankCardsData()
        {
            this.BankCards = this.State.BankCards.ToDictionary(c => c.BankCardNo);
        }

        private void ReloadJBYAccountData()
        {
            List<Transcation> debitTrans = this.State.SettleAccount.Where(t => t.Trade == Trade.Debit && t.ResultCode == 1).ToList();
            List<Transcation> creditTrans = this.State.SettleAccount.Where(t => t.Trade == Trade.Credit && t.ResultCode >= 0).ToList();
            List<Transcation> creditedTrans = this.State.SettleAccount.Where(t => t.Trade == Trade.Credit && t.ResultCode == 1).ToList();
            // ReSharper disable once UnusedVariable
            List<Transcation> creditingTrans = this.State.SettleAccount.Where(t => t.Trade == Trade.Credit && t.ResultCode == 0).ToList();

            this.JBYAccrualAmount = debitTrans.Sum(t => t.Amount) - creditedTrans.Sum(t => t.Amount);
            this.JBYWithdrawalableAmount = debitTrans.Sum(t => t.Amount) - creditTrans.Sum(t => t.Amount);

            DateTime todayDate = DateTime.UtcNow.AddHours(8).Date;
            this.TodayJBYWithdrawalAmount = this.State.JBYAccount.Count(t => t.TransactionTime >= todayDate && t.TransactionTime < todayDate.AddDays(1) && t.TradeCode == TradeCodeHelper.TC2001012002);

            this.JBYAccount = this.State.JBYAccount.ToDictionary(t => t.TransactionId);
        }

        /// <summary>
        ///     Reloads the order infos data.
        /// </summary>
        private void ReloadOrderInfosData()
        {
            List<OrderInfo> paidOrders = this.State.Orders.Where(o => o.ResultCode == 1).ToList();
            List<OrderInfo> investingOrders = paidOrders.Where(o => !o.IsRepaid).ToList();

            this.TotalPrincipal = paidOrders.Sum(o => o.Principal);
            this.TotalInterest = paidOrders.Sum(o => o.Interest + o.ExtraInterest);
            this.InvestingPrincipal = investingOrders.Sum(o => o.Principal);
            this.InvestingInterest = investingOrders.Sum(o => o.Interest + o.ExtraInterest);

            this.Orders = this.State.Orders.ToDictionary(o => o.OrderId);
        }

        /// <summary>
        ///     Reloads the settle account data.
        /// </summary>
        private void ReloadSettleAccountData()
        {
            List<Transcation> debitTrans = this.State.SettleAccount.Where(t => t.Trade == Trade.Debit && t.ResultCode == 1).ToList();
            List<Transcation> creditTrans = this.State.SettleAccount.Where(t => t.Trade == Trade.Credit && t.ResultCode >= 0).ToList();

            this.SettleAccountBalance = debitTrans.Sum(t => t.Amount) - creditTrans.Sum(t => t.Amount);
            this.DebitingSettleAccountAmount = this.State.SettleAccount.Where(t => t.ResultCode == 0 && t.Trade == Trade.Debit).Sum(t => t.Amount);
            this.CreditingSettleAccountAmount = this.State.SettleAccount.Where(t => t.ResultCode == 0 && t.Trade == Trade.Credit).Sum(t => t.Amount);

            this.BankCards.Values.ForEach(c =>
            {
                c.WithdrawAmount = debitTrans.Where(t => t.BankCardNo == c.BankCardNo).Sum(t => t.Amount)
                                   - creditTrans.Where(t => t.BankCardNo == c.BankCardNo).Sum(t => t.Amount);
            });

            DateTime todayDate = DateTime.UtcNow.AddHours(8).Date;
            DateTime monthDate = new DateTime(todayDate.Year, todayDate.Month, 1);

            this.TodayWithdrawalCount = this.State.SettleAccount.Count(t => t.TransactionTime >= todayDate && t.TransactionTime < todayDate.AddDays(1) && t.TradeCode == TradeCodeHelper.TC1005052001);
            this.MonthWithdrawalCount = this.State.SettleAccount.Count(t => t.TransactionTime >= monthDate && t.TransactionTime < monthDate.AddMonths(1) && t.TradeCode == TradeCodeHelper.TC1005052001);

            this.SettleAccount = this.State.SettleAccount.ToDictionary(t => t.TransactionId);
        }
    }
}