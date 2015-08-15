// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : JBYProductWithdrawalManager.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-14  19:38
// ***********************************************************************
// <copyright file="JBYProductWithdrawalManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans.Providers;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     JBYProductWithdrawalManager.
    /// </summary>
    [StorageProvider(ProviderName = "SqlDatabase")]
    public class JBYProductWithdrawalManager : EntityGrain<IJBYProductWithdrawalManagerState>, IJBYProductWithdrawalManager
    {
        private static Tuple<DateTime, DailyConfig> todayConfig = new Tuple<DateTime, DailyConfig>(DateTime.MinValue, null);
        private long WithdrawalAmount { get; set; }

        #region IJBYProductWithdrawalManager Members

        /// <summary>
        ///     Builds the withdrawal transaction.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<DateTime?> BuildWithdrawalTransactionAsync(JBYAccountTransactionInfo info)
        {
            JBYAccountTransactionInfo transaction;
            if (!this.State.WithdrawalTransactions.TryGetValue(info.TransactionId, out transaction))
            {
                DateTime predeterminedResultDate = this.GetPredeterminedResultDate(info.Amount);

                transaction = new JBYAccountTransactionInfo
                {
                    Amount = info.Amount,
                    Args = info.Args,
                    PredeterminedResultDate = predeterminedResultDate,
                    ProductId = info.ProductId,
                    ResultCode = info.ResultCode,
                    ResultTime = info.ResultTime,
                    SettleAccountTransactionId = info.SettleAccountTransactionId,
                    Trade = info.Trade,
                    TradeCode = info.TradeCode,
                    TransDesc = info.TransDesc,
                    TransactionId = info.TransactionId,
                    TransactionTime = info.TransactionTime,
                    UserId = info.UserId,
                    UserInfo = info.UserInfo
                };

                this.State.WithdrawalTransactions.Add(transaction.TransactionId, transaction);

                await this.SaveStateAsync();

                this.ReloadTransactionData();
            }

            return transaction.PredeterminedResultDate;
        }

        #endregion IJBYProductWithdrawalManager Members

        private DateTime GetPredeterminedResultDate(long amount)
        {
            DateTime predeterminedResultDate = DateTime.UtcNow.ToChinaStandardTime().Date;
            DailyConfig dailyConfig = GetTodayConfig();

            if (this.WithdrawalAmount + amount <= dailyConfig.JBYWithdrawalLimit)
            {
                int waitingDays = (int)((this.WithdrawalAmount + amount) / dailyConfig.JBYWithdrawalLimit) + 1;

                DailyConfig config = DailyConfigHelper.GetNextWorkDayConfig(waitingDays - 1);

                predeterminedResultDate = config.Date;
            }
            return predeterminedResultDate;
        }

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.RegisterTimer(o => this.RemovePastTransactionsAsync(), new object(), TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));

            this.ReloadTransactionData();

            return base.OnActivateAsync();
        }

        private static DailyConfig GetTodayConfig()
        {
            if (todayConfig.Item1 < DateTime.UtcNow.AddHours(8).Date.AddMinutes(20) || todayConfig.Item1 < DateTime.UtcNow.AddHours(8).AddMinutes(-10))
            {
                DailyConfig config = DailyConfigHelper.GetTodayDailyConfig();

                todayConfig = new Tuple<DateTime, DailyConfig>(DateTime.UtcNow.AddHours(8), config);
            }

            return todayConfig.Item2;
        }

        private void ReloadTransactionData()
        {
            this.WithdrawalAmount = this.State.WithdrawalTransactions.Values
                .Where(t => !t.PredeterminedResultDate.HasValue || t.PredeterminedResultDate.Value.Date < DateTime.UtcNow.AddHours(8)).Sum(t => t.Amount);
        }

        private async Task RemovePastTransactionsAsync()
        {
            List<Guid> toRemove = this.State.WithdrawalTransactions.Values
                .Where(t => !t.PredeterminedResultDate.HasValue || t.PredeterminedResultDate.Value.Date < DateTime.UtcNow.AddHours(8))
                .Select(t => t.TransactionId).ToList();

            foreach (Guid id in toRemove)
            {
                this.State.WithdrawalTransactions.Remove(id);
            }

            this.ReloadTransactionData();

            await this.SaveStateAsync();
        }
    }
}