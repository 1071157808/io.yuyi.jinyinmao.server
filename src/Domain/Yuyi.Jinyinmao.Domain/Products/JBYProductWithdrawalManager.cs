// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  1:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  10:53 PM
// ***********************************************************************
// <copyright file="JBYProductWithdrawalManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orleans;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     JBYProductWithdrawalManager.
    /// </summary>
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
        public Task<DateTime?> BuildWithdrawalTranscationAsync(JBYAccountTranscationInfo info)
        {
            DailyConfig dailyConfig = GetTodayConfig();
            JBYAccountTranscationInfo transcation;
            if (!this.State.WithdrawalTranscations.TryGetValue(info.TransactionId, out transcation))
            {
                transcation = info;

                int waitingDays = (int)((this.WithdrawalAmount + info.Amount) / dailyConfig.JBYWithdrawalLimit) + 1;

                DailyConfig config = DailyConfigHelper.GetNextWorkDayConfig(waitingDays);

                transcation.PredeterminedResultDate = config.Date.Date;

                this.State.WithdrawalTranscations.Add(transcation.TransactionId, transcation);

                this.ReloadTranscationData();
            }

            return Task.FromResult(transcation.PredeterminedResultDate);
        }

        #endregion IJBYProductWithdrawalManager Members

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.RemovePastTranscations();

            this.RegisterTimer(o => this.RemovePastTranscations(), new object(), TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));

            this.ReloadTranscationData();

            return base.OnActivateAsync();
        }

        private Task RemovePastTranscations()
        {
            List<Guid> toRemove = this.State.WithdrawalTranscations.Values
                .Where(t => !t.PredeterminedResultDate.HasValue || t.PredeterminedResultDate.Value.Date < DateTime.UtcNow.AddHours(8))
                .Select(t => t.TransactionId).ToList();

            foreach (Guid id in toRemove)
            {
                this.State.WithdrawalTranscations.Remove(id);
            }

            return TaskDone.Done;
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

        private void ReloadTranscationData()
        {
            this.WithdrawalAmount = this.State.WithdrawalTranscations.Values.Sum(t => Convert.ToInt64(t.Amount));
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
    }
}