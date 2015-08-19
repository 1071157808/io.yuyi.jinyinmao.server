// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : User_ReloadData.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-19  20:23
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
        /// <summary>
        ///     Reloads the jby account data.
        /// </summary>
        private void ReloadJBYAccountData()
        {
            DateTime now = DateTime.UtcNow.ToChinaStandardTime();
            DateTime today = now.Date;

            long debitTransAmount = 0L;
            long debitedTransAmount = 0L;
            long creditedTransAmount = 0L;
            long creditingTransAmount = 0L;

            long todayJBYWithdrawalAmount = 0L;
            long jbyTotalInterest = 0L;

            long todayInvestingAmount = 0L;

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
                            jbyTotalInterest += transaction.Amount;
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

                    if (transaction.TransactionTime >= today && transaction.TransactionTime < today.AddDays(1) && transaction.ResultCode >= 0)
                    {
                        todayJBYWithdrawalAmount += transaction.Amount;
                    }
                }
            }

            this.JBYTotalAmount = debitTransAmount - creditedTransAmount;
            this.JBYWithdrawalableAmount = debitedTransAmount - todayInvestingAmount - creditedTransAmount - creditingTransAmount;

            this.JBYTotalInterest = jbyTotalInterest;
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
            this.UpdateOrders();

            long totalPrincipal = 0L;
            long totalInterest = 0L;
            long investingPrincipal = 0L;
            long investingInterest = 0L;
            long yinInvestingPrincipal = 0L;
            long yinInvestingInterest = 0L;
            long shangInvestingPrincipal = 0L;
            long shangInvestingInterest = 0L;
            long bankInvestingPrincipal = 0L;
            long bankInvestingInterest = 0L;

            foreach (Order order in this.State.Orders.Values.Where(order => order.ResultCode > 0))
            {
                totalPrincipal += order.Principal;
                totalInterest += (order.Interest + order.ExtraInterest);

                if (!order.IsRepaid)
                {
                    long principal = order.Principal;
                    long interest = order.Interest + order.ExtraInterest;

                    investingPrincipal += principal;
                    investingInterest += interest;

                    if (order.ProductCategory == ProductCategoryCodeHelper.PC100000010)
                    {
                        yinInvestingPrincipal += principal;
                        yinInvestingInterest += interest;
                    }
                    else if (order.ProductCategory == ProductCategoryCodeHelper.PC100000020)
                    {
                        shangInvestingPrincipal += principal;
                        shangInvestingInterest += interest;
                    }
                    else if (ProductCategoryCodeHelper.IsBankRegularProduct(order.ProductCategory))
                    {
                        bankInvestingPrincipal += principal;
                        bankInvestingInterest += interest;
                    }
                }
            }

            this.TotalPrincipal = totalPrincipal;
            this.TotalInterest = totalInterest;
            this.InvestingPrincipal = investingPrincipal;
            this.InvestingInterest = investingInterest;
            this.YinInvestingPrincipal = yinInvestingPrincipal;
            this.YinInvestingInterest = yinInvestingInterest;
            this.ShangInvestingPrincipal = shangInvestingPrincipal;
            this.ShangInvestingInterest = shangInvestingInterest;
            this.BankInvestingPrincipal = bankInvestingPrincipal;
            this.BankInvestingInterest = bankInvestingInterest;
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

            DateTime todayDate = DateTime.UtcNow.ToChinaStandardTime().Date;
            DateTime monthDate = new DateTime(todayDate.Year, todayDate.Month, 1).Date;

            foreach (SettleAccountTransaction transaction in this.State.SettleAccount.Values.OrderBy(t => t.TransactionTime))
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
                            if (bankCards[bankCardNo] < 0)
                            {
                                bankCards[bankCardNo] = 0L;
                            }
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

        private void UpdateOrders()
        {
            DateTime now = DateTime.UtcNow.ToChinaStandardTime().AddDays(-2);
            foreach (KeyValuePair<Guid, Order> order in this.State.Orders.Where(order => !order.Value.IsRepaid && order.Value.ProductSnapshot.RepaymentDeadline <= now))
            {
                order.Value.IsRepaid = true;
                order.Value.RepaidTime = order.Value.ProductSnapshot.RepaymentDeadline;
            }

            this.SaveStateAsync();
        }
    }
}