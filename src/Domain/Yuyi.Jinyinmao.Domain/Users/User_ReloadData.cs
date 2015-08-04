// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : User_ReloadData.cs
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-04  1:42 PM
// ***********************************************************************
// <copyright file="User_ReloadData.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Moe.Lib;
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
                DailyConfig confirmConfig = DailyConfigHelper.GetLastWorkDayConfig(todayConfig.IsWorkDay ? 0 : 1);

                lastInvestingConfirmTime = new Tuple<DateTime, DateTime>(DateTime.UtcNow.AddHours(8), confirmConfig.Date.Date.AddDays(1).AddMilliseconds(-1));
            }

            return lastInvestingConfirmTime.Item2;
        }

        private void ReloadJBYAccountData()
        {
            DateTime todayDate = DateTime.UtcNow.AddHours(8).Date;

            long debitTransAmount = 0L;
            long debitedTransAmount = 0L;
            long creditedTransAmount = 0L;
            long creditingTransAmount = 0L;

            long todayJBYWithdrawalAmount = 0L;
            long jBYTotalInterest = 0L;

            long todayInvestingAmount = 0L;

            DateTime now = DateTime.UtcNow.AddHours(8);
            DateTime today = now.Date;

            foreach (JBYAccountTransaction transaction in this.State.JBYAccount.Values)
            {
                if (transaction.Trade == Trade.Debit)
                {
                    debitTransAmount += transaction.Amount;

                    if (transaction.ResultCode > 0)
                    {
                        debitedTransAmount += transaction.Amount;

                        if (transaction.TradeCode == TradeCodeHelper.TC2001011106)
                        {
                            jBYTotalInterest += transaction.Amount;
                        }

                        if (transaction.ResultTime.GetValueOrDefault().Date == today)
                        {
                            todayInvestingAmount += transaction.Amount;
                        }
                    }
                }
                else
                {
                    if (transaction.ResultCode > 0)
                    {
                        creditedTransAmount += transaction.Amount;
                    }
                    else if (transaction.ResultCode == 0)
                    {
                        creditingTransAmount += transaction.Amount;
                    }

                    if (transaction.TransactionTime >= todayDate && transaction.TransactionTime < todayDate.AddDays(1) && transaction.ResultCode >= 0)
                    {
                        todayJBYWithdrawalAmount += transaction.Amount;
                    }
                }
            }

            this.JBYTotalAmount = debitTransAmount - creditedTransAmount;
            this.JBYWithdrawalableAmount = debitedTransAmount - todayInvestingAmount - creditedTransAmount - creditingTransAmount;

            this.JBYTotalInterest = jBYTotalInterest;
            this.JBYTotalPricipal = debitedTransAmount;

            this.TodayJBYWithdrawalAmount = todayJBYWithdrawalAmount;

            this.JBYAccrualAmount = this.GetJBYAccrualAmount(now);
            JBYAccountTransaction lastReinvesting = this.State.JBYAccount.Values.Where(t => t.TradeCode == TradeCodeHelper.TC2001011106 && t.ResultCode > 0)
                .OrderByDescending(t => t.ResultTime.GetValueOrDefault(DateTime.MinValue)).FirstOrDefault();

            if (lastReinvesting == null || lastReinvesting.ResultTime.GetValueOrDefault() <= now.AddHours(-22))
            {
                this.JBYLastInterest = 0L;
            }
            else
            {
                this.JBYLastInterest = lastReinvesting.Amount;
            }
        }

        /// <summary>
        ///     Reloads the order infos data.
        /// </summary>
        private void ReloadOrderInfosData()
        {
            long totalPrincipal = 0;
            long totalInterest = 0;
            long investingPrincipal = 0;
            long investingInterest = 0;

            foreach (Order order in this.State.Orders.Values.Where(order => order.ResultCode > 0))
            {
                totalPrincipal += order.Principal;
                totalInterest += (order.Interest + order.ExtraInterest);

                if (!order.IsRepaid)
                {
                    investingPrincipal += order.Principal;
                    investingInterest += (order.Interest + order.ExtraInterest);
                }
            }

            this.TotalPrincipal = totalPrincipal;
            this.TotalInterest = totalInterest;
            this.InvestingPrincipal = investingPrincipal;
            this.InvestingInterest = investingInterest;
        }

        /// <summary>
        ///     Reloads the settle account data.
        /// </summary>
        private void ReloadSettleAccountData()
        {
            long settleAccountBalance = 0L;
            long debitingSettleAccountAmount = 0L;
            long creditingSettleAccountAmount = 0L;
            int todayWithdrawalCount = 0;
            int monthWithdrawalCount = 0;

            Dictionary<string, long> bankCards = this.State.BankCards.Where(kv => kv.Value.Dispaly && kv.Value.Verified).ToDictionary(kv => kv.Key, kv => 0L);

            DateTime todayDate = DateTime.UtcNow.AddHours(8).Date;
            DateTime monthDate = new DateTime(todayDate.Year, todayDate.Month, 1).Date;

            foreach (SettleAccountTransaction transaction in this.State.SettleAccount.Values)
            {
                long amount = transaction.Amount;
                string bankCardNo = transaction.BankCardNo;
                if (transaction.Trade == Trade.Debit)
                {
                    if (transaction.ResultCode > 0)
                    {
                        settleAccountBalance += amount;

                        if (transaction.TradeCode == TradeCodeHelper.TC1005051001 && transaction.BankCardNo.IsNotNullOrEmpty() && bankCards.ContainsKey(bankCardNo))
                        {
                            bankCards[bankCardNo] += amount;
                        }
                    }
                    else if (transaction.ResultCode == 0)
                    {
                        debitingSettleAccountAmount += amount;
                    }
                }
                else if (transaction.Trade == Trade.Credit && transaction.ResultCode >= 0)
                {
                    settleAccountBalance -= amount;

                    if (transaction.TradeCode == TradeCodeHelper.TC1005052001)
                    {
                        if (transaction.TransactionTime >= todayDate && transaction.TransactionTime < todayDate.AddDays(1))
                        {
                            todayWithdrawalCount += 1;
                        }

                        if (transaction.TransactionTime >= monthDate && transaction.TransactionTime < monthDate.AddMonths(1))
                        {
                            monthWithdrawalCount += 1;
                        }

                        if (transaction.BankCardNo.IsNotNullOrEmpty() && bankCards.ContainsKey(bankCardNo))
                        {
                            bankCards[bankCardNo] -= amount;
                        }
                    }

                    if (transaction.ResultCode == 0)
                    {
                        creditingSettleAccountAmount += amount;
                    }
                }
            }

            this.SettleAccountBalance = settleAccountBalance;
            this.DebitingSettleAccountAmount = debitingSettleAccountAmount;
            this.CreditingSettleAccountAmount = creditingSettleAccountAmount;

            long incomingAmount = bankCards.Sum(kv => kv.Value);
            long extraAmount = settleAccountBalance - incomingAmount;
            extraAmount = extraAmount > 0 ? extraAmount : 0;
            foreach (BankCard bankCard in this.State.BankCards.Values)
            {
                if (bankCard.Dispaly && bankCard.Verified)
                {
                    bankCard.WithdrawAmount = bankCards[bankCard.BankCardNo] + extraAmount;
                    if (bankCard.WithdrawAmount < 0)
                    {
                        bankCard.WithdrawAmount = 0L;
                    }
                }
                else
                {
                    bankCard.WithdrawAmount = 0L;
                }
            }

            this.TodayWithdrawalCount = todayWithdrawalCount;
            this.MonthWithdrawalCount = monthWithdrawalCount;
        }
    }
}