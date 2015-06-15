// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-27  7:39 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-06-15  6:46 PM
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
            DateTime confirmTime = GetLastInvestingConfirmTime();
            DateTime todayDate = DateTime.UtcNow.AddHours(8).Date;

            long debitTransAmount = 0L;
            long debitedTransAmount = 0L;
            long creditedTransAmount = 0L;
            long creditingTransAmount = 0L;

            long todayJBYWithdrawalAmount = 0L;
            long investingConfirmTransAmount = 0L;
            long jBYTotalInterest = 0L;

            foreach (JBYAccountTranscation transcation in this.State.JBYAccount.Values)
            {
                if (transcation.Trade == Trade.Debit)
                {
                    debitTransAmount += transcation.Amount;

                    if (transcation.ResultCode > 0)
                    {
                        debitedTransAmount += transcation.Amount;
                        if (transcation.ResultTime.GetValueOrDefault(DateTime.MaxValue) <= confirmTime)
                        {
                            investingConfirmTransAmount += transcation.Amount;
                        }

                        if (transcation.ProductId == SpecialIdHelper.ReinvestingJBYTranscationProductId)
                        {
                            jBYTotalInterest += transcation.Amount;
                        }
                    }
                }
                else
                {
                    if (transcation.ResultCode > 0)
                    {
                        creditedTransAmount += transcation.Amount;
                    }
                    else if (transcation.ResultCode == 0)
                    {
                        creditingTransAmount += transcation.Amount;
                    }

                    if (transcation.TransactionTime >= todayDate && transcation.TransactionTime < todayDate.AddDays(1) && transcation.ResultCode >= 0)
                    {
                        todayJBYWithdrawalAmount += transcation.Amount;
                    }
                }
            }

            this.JBYAccrualAmount = investingConfirmTransAmount - creditedTransAmount;
            this.JBYTotalAmount = debitTransAmount - creditedTransAmount;
            this.JBYWithdrawalableAmount = this.JBYAccrualAmount - creditingTransAmount;

            this.JBYTotalInterest = jBYTotalInterest;
            this.JBYTotalPricipal = debitedTransAmount;

            this.TodayJBYWithdrawalAmount = todayJBYWithdrawalAmount;
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
            long settleAccountBalance = 0;
            long debitingSettleAccountAmount = 0;
            long creditingSettleAccountAmount = 0;
            int todayWithdrawalCount = 0;
            int monthWithdrawalCount = 0;

            Dictionary<string, long> bankCards = this.State.BankCards.ToDictionary(kv => kv.Key, kv => 0L);
            bankCards.Add(string.Empty, 0);

            DateTime todayDate = DateTime.UtcNow.AddHours(8).Date;
            DateTime monthDate = new DateTime(todayDate.Year, todayDate.Month, 1).Date;

            foreach (SettleAccountTranscation transcation in this.State.SettleAccount.Values)
            {
                long amount = transcation.Amount;
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
                else if (transcation.Trade == Trade.Credit && transcation.ResultCode >= 0)
                {
                    settleAccountBalance -= amount;
                    bankCards[bankCardNo] -= amount;

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