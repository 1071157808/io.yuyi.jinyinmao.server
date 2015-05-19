// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-07  12:19 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  11:06 PM
// ***********************************************************************
// <copyright file="User_ReloadData.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
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
        private static Tuple<DateTime, DateTime> lastInvestingConfirmTime = new Tuple<DateTime, DateTime>(DateTime.MinValue, DateTime.UtcNow);

        private static DateTime GetLastInvestingConfirmTime()
        {
            if (lastInvestingConfirmTime.Item1 < DateTime.UtcNow.AddHours(8).Date.AddMinutes(20))
            {
                DailyConfig todayConfig = DailyConfigHelper.GetTodayDailyConfig();
                DailyConfig confirmConfig = DailyConfigHelper.GetLastWorkDayConfig(todayConfig.IsWorkDay ? 0 : -1);

                lastInvestingConfirmTime = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddHours(8), confirmConfig.Date.Date.AddDays(1).AddMilliseconds(-1));
            }

            return lastInvestingConfirmTime.Item2;
        }

        private void ReloadJBYAccountData()
        {
            List<JBYAccountTranscation> debitTrans = this.State.JBYAccount.Values.Where(t => t.Trade == Trade.Debit && t.ResultCode >= 0).ToList();
            List<JBYAccountTranscation> debitedTrans = debitTrans.Where(t => t.ResultCode > 0).ToList();
            List<JBYAccountTranscation> creditTrans = this.State.JBYAccount.Values.Where(t => t.Trade == Trade.Credit && t.ResultCode >= 0).ToList();
            List<JBYAccountTranscation> creditedTrans = creditTrans.Where(t => t.ResultCode > 0).ToList();
            List<JBYAccountTranscation> creditingTrans = creditTrans.Where(t => t.ResultCode == 0).ToList();

            DateTime confirmTime = GetLastInvestingConfirmTime();

            this.JBYAccrualAmount = debitTrans.Where(t => t.TransactionTime <= confirmTime).Sum(t => t.Amount) - creditedTrans.Sum(t => t.Amount);
            this.JBYTotalAmount = debitTrans.Sum(t => t.Amount) - creditedTrans.Sum(t => t.Amount);
            this.JBYWithdrawalableAmount = this.JBYAccrualAmount - creditingTrans.Sum(t => t.Amount);

            this.JBYTotalInterest = debitTrans.Where(t => t.ProductId == Guid.Empty).Sum(t => t.Amount);
            this.JBYTotalPricipal = debitedTrans.Sum(t => t.Amount);

            DateTime todayDate = DateTime.UtcNow.AddHours(8).Date;
            this.TodayJBYWithdrawalAmount = creditTrans.Where(t => t.TransactionTime >= todayDate && t.TransactionTime < todayDate.AddDays(1))
                .Sum(t => t.Amount);
        }

        /// <summary>
        ///     Reloads the order infos data.
        /// </summary>
        private void ReloadOrderInfosData()
        {
            List<OrderInfo> paidOrders = this.State.Orders.Values.Where(o => o.ResultCode == 1).ToList();
            List<OrderInfo> investingOrders = paidOrders.Where(o => !o.IsRepaid).ToList();

            this.TotalPrincipal = paidOrders.Sum(o => o.Principal);
            this.TotalInterest = paidOrders.Sum(o => o.Interest + o.ExtraInterest);
            this.InvestingPrincipal = investingOrders.Sum(o => o.Principal);
            this.InvestingInterest = investingOrders.Sum(o => o.Interest + o.ExtraInterest);
        }

        /// <summary>
        ///     Reloads the settle account data.
        /// </summary>
        private void ReloadSettleAccountData()
        {
            List<SettleAccountTranscation> transcations = this.State.SettleAccount.Values.Where(t => t.ResultCode >= 0).ToList();
            int settleAccountBalance = 0;
            int debitingSettleAccountAmount = 0;
            int creditingSettleAccountAmount = 0;
            int todayWithdrawalCount = 0;
            int monthWithdrawalCount = 0;

            Dictionary<string, int> bankCards = this.State.BankCards.ToDictionary(kv => kv.Key, kv => 0);
            bankCards.Add(string.Empty, 0);

            DateTime todayDate = DateTime.UtcNow.AddHours(8).Date;
            DateTime monthDate = new DateTime(todayDate.Year, todayDate.Month, 1).Date;

            foreach (SettleAccountTranscation transcation in transcations)
            {
                int amount = transcation.Amount;
                string bankCardNo = transcation.BankCardNo;
                if (transcation.Trade == Trade.Debit)
                {
                    if (transcation.ResultCode > 0)
                    {
                        settleAccountBalance += amount;
                        bankCards[bankCardNo] += amount;
                    }
                    else if (transcation.ResultCode == 0)
                    {
                        debitingSettleAccountAmount += amount;
                    }
                }
                else if (transcation.Trade == Trade.Credit)
                {
                    if (transcation.TradeCode == TradeCodeHelper.TC1005052001)
                    {
                        if (transcation.TransactionTime >= todayDate && transcation.TransactionTime < todayDate.AddDays(1))
                        {
                            todayWithdrawalCount += 1;
                        }

                        if (transcation.TransactionTime >= monthDate && transcation.TransactionTime < monthDate.AddMonths(1))
                        {
                            monthWithdrawalCount += 1;
                        }
                    }

                    if (transcation.ResultCode == 0)
                    {
                        creditingSettleAccountAmount += amount;
                    }

                    settleAccountBalance -= amount;
                    bankCards[bankCardNo] -= amount;
                }
            }

            this.SettleAccountBalance = settleAccountBalance;
            this.DebitingSettleAccountAmount = debitingSettleAccountAmount;
            this.CreditingSettleAccountAmount = creditingSettleAccountAmount;

            this.State.BankCards.Values.ForEach(c => c.WithdrawAmount = bankCards[c.BankCardNo]);

            this.TodayWithdrawalCount = todayWithdrawalCount;
            this.MonthWithdrawalCount = monthWithdrawalCount;
        }
    }
}