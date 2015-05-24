// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  1:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-25  2:09 AM
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
        ///     Builds the withdrawal transcation.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<DateTime?> BuildWithdrawalTranscationAsync(JBYAccountTranscationInfo info)
        {
            DailyConfig dailyConfig = GetTodayConfig();
            JBYAccountTranscationInfo transcation;
            if (!this.State.WithdrawalTranscations.TryGetValue(info.TransactionId, out transcation))
            {
                int waitingDays = (int)((this.WithdrawalAmount + info.Amount) / dailyConfig.JBYWithdrawalLimit) + 1;

                DailyConfig config = DailyConfigHelper.GetNextWorkDayConfig(waitingDays - 1);

                transcation = new JBYAccountTranscationInfo
                {
                    Amount = info.Amount,
                    Args = info.Args,
                    PredeterminedResultDate = config.Date,
                    ProductId = info.ProductId,
                    ResultCode = info.ResultCode,
                    ResultTime = info.ResultTime,
                    SettleAccountTranscationId = info.SettleAccountTranscationId,
                    Trade = info.Trade,
                    TradeCode = info.TradeCode,
                    TransDesc = info.TransDesc,
                    TransactionId = info.TransactionId,
                    TransactionTime = info.TransactionTime,
                    UserId = info.UserId
                };

                this.State.WithdrawalTranscations.Add(transcation.TransactionId, transcation);

                await this.SaveStateAsync();

                this.ReloadTranscationData();
            }

            return transcation.PredeterminedResultDate;
        }

        #endregion IJBYProductWithdrawalManager Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.RegisterTimer(o => this.RemovePastTranscationsAsync(), new object(), TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));

            this.ReloadTranscationData();

            return base.OnActivateAsync();
        }

        /// <summary>
        ///     Reload state data as an asynchronous operation.
        /// </summary>
        /// <returns>Task.</returns>
        public override async Task ReloadAsync()
        {
            await this.State.ReadStateAsync();
            this.ReloadTranscationData();
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

        private void ReloadTranscationData()
        {
            this.WithdrawalAmount = this.State.WithdrawalTranscations.Values
                .Where(t => !t.PredeterminedResultDate.HasValue || t.PredeterminedResultDate.Value.Date < DateTime.UtcNow.AddHours(8)).Sum(t => Convert.ToInt64(t.Amount));
        }

        private async Task RemovePastTranscationsAsync()
        {
            List<Guid> toRemove = this.State.WithdrawalTranscations.Values
                .Where(t => !t.PredeterminedResultDate.HasValue || t.PredeterminedResultDate.Value.Date < DateTime.UtcNow.AddHours(8))
                .Select(t => t.TransactionId).ToList();

            foreach (Guid id in toRemove)
            {
                this.State.WithdrawalTranscations.Remove(id);
            }

            this.ReloadTranscationData();

            await this.SaveStateAsync();
        }
    }
}