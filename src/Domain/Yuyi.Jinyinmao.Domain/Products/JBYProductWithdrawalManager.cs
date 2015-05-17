// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  1:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-12  1:27 AM
// ***********************************************************************
// <copyright file="JBYProductWithdrawalManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yuyi.Jinyinmao.Domain.Dtos;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain.Products
{
    /// <summary>
    ///     JBYProductWithdrawalManager.
    /// </summary>
    public class JBYProductWithdrawalManager : EntityGrain<IJBYProductWithdrawalManagerState>, IJBYProductWithdrawalManager
    {
        private long WithdrawalAmount { get; set; }

        private List<Tuple<int, TranscationInfo>> WithdrawalTranscations { get; set; }

        private static readonly string WaitingDaysKeyName = "WaitingDays";

        /// <summary>
        ///     This method is called at the end of the process of activating a grain.
        ///     It is called before any messages have been dispatched to the grain.
        ///     For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            this.State.TodayConfig = DailyConfigHelper.GetTodayDailyConfig();
            this.State.LastWorkDayConfig = DailyConfigHelper.GetLastWorkDayConfig();

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

        private void ReloadTranscationData()
        {
            this.WithdrawalTranscations = this.State.WithdrawalTranscations.Where(t => t.ResultCode == 0)
                .OrderBy(t => t.TransactionTime).ThenBy(t => t.Amount)
                .Select(t => new Tuple<int, TranscationInfo>(Convert.ToInt32(t.Info[WaitingDaysKeyName]), t)).ToList();

            this.WithdrawalAmount = this.WithdrawalTranscations.Sum(t => Convert.ToInt64(t.Item2.Amount));
        }

        /// <summary>
        /// Builds the withdrawal transcation.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<int> BuildWithdrawalTranscationAsync(TranscationInfo info)
        {
            int waitingDays = (int)((this.WithdrawalAmount + info.Amount) / this.State.TodayConfig.JBYWithdrawalLimit) + 1;

            if (info.Info.ContainsKey(WaitingDaysKeyName))
            {
                info.Info[WaitingDaysKeyName] = waitingDays;
            }
            else
            {
                info.Info.Add(WaitingDaysKeyName, waitingDays);
            }

            this.ReloadTranscationData();

            return Task.FromResult(waitingDays);
        }
    }
}