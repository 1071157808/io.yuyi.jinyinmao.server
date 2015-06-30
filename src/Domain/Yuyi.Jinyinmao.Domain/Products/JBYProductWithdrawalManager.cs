// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  1:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  1:29 PM
// ***********************************************************************
// <copyright file="JBYProductWithdrawalManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            DailyConfig dailyConfig = GetTodayConfig();
            JBYAccountTransactionInfo transaction;
            if (!this.State.WithdrawalTransactions.TryGetValue(info.TransactionId, out transaction))
            {
                int waitingDays = (int)((this.WithdrawalAmount + info.Amount) / dailyConfig.JBYWithdrawalLimit) + 1;

                DailyConfig config = DailyConfigHelper.GetNextWorkDayConfig(waitingDays - 1);

                transaction = new JBYAccountTransactionInfo
                {
                    Amount = info.Amount,
                    Args = info.Args,
                    PredeterminedResultDate = config.Date,
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

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ReloadAsync()
        {
            await this.State.ReadStateAsync();
            this.ReloadTransactionData();
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
                .Where(t => !t.PredeterminedResultDate.HasValue || t.PredeterminedResultDate.Value.Date < DateTime.UtcNow.AddHours(8)).Sum(t => Convert.ToInt64(t.Amount));
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