// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-12  1:00 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-18  2:03 PM
// ***********************************************************************
// <copyright file="JBYProductWithdrawalManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
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

        #region IJBYProductWithdrawalManager Members

        /// <summary>
        ///     Builds the withdrawal transcation.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<int?> BuildWithdrawalTranscationAsync(JBYAccountTranscationInfo info)
        {
            Tuple<int, JBYAccountTranscationInfo> transcation;
            if (this.State.WithdrawalTranscations.TryGetValue(info.TransactionId, out transcation))
            {
                return Task.FromResult<int?>(transcation.Item1);
            }

            int waitingDays = (int)((this.WithdrawalAmount + info.Amount) / this.State.TodayConfig.JBYWithdrawalLimit) + 1;

            this.State.WithdrawalTranscations.Add(info.TransactionId, new Tuple<int, JBYAccountTranscationInfo>(waitingDays, info));

            this.ReloadTranscationData();

            return Task.FromResult<int?>(waitingDays);
        }

        #endregion IJBYProductWithdrawalManager Members

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
            this.WithdrawalAmount = this.State.WithdrawalTranscations.Values.Sum(t => Convert.ToInt64(t.Item2.Amount));
        }
    }
}