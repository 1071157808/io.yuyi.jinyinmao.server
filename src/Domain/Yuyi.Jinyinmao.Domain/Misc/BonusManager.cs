// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : BonusManager.cs
// Created          : 2015-08-12  1:03 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-12  1:07 PM
// ***********************************************************************
// <copyright file="BonusManager.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Moe.Lib;
using Orleans;
using Yuyi.Jinyinmao.Domain.Models;
using Yuyi.Jinyinmao.Packages.Helper;

namespace Yuyi.Jinyinmao.Domain.Misc
{
    /// <summary>
    /// BonusManager.
    /// </summary>
    public class BonusManager : Grain, IBonusManager
    {
        /// <summary>
        /// The maximum bonus amount
        /// </summary>
        private static readonly long MaxBonusAmount = 10000L;

        /// <summary>
        /// The minimum bonus amount
        /// </summary>
        private static readonly long MinBonusAmount = 1L;

        /// <summary>
        /// The upper limit
        /// </summary>
        private static readonly int UpperLimit = 1000;

        /// <summary>
        /// The floor limit
        /// </summary>
        private static readonly int FloorLimit = 100;

        private static readonly Random Random = new Random(DateTime.Now.DayOfYear);

        #region IBonusManager Members

        /// <summary>
        ///     Gets the bonus amount.
        /// </summary>
        /// <param name="baseAmount">The base amount.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public Task<long> GetBonusAmount(long baseAmount)
        {
            int r = Random.Next(FloorLimit, UpperLimit + 1);

            long bonus = baseAmount * r / 36000000;

            if (bonus <= MinBonusAmount)
            {
                bonus = MinBonusAmount;
            }
            else if (bonus > MaxBonusAmount)
            {
                bonus = MaxBonusAmount;
            }

            return Task.FromResult(bonus);
        }

        private async Task<long> GetRemainBonusAmount()
        {
            if (!this.RemainWithdrawalAmount.HasValue)
            {
                DateTime today = DateTime.UtcNow.ToChinaStandardTime().Date;
                long bonusAmount = GetTodayConfig().BonusAmount;
                using (JYMDBContext db = new JYMDBContext())
                {
                    long givenBonusAmount = await db.ReadonlyQuery<SettleAccountTransaction>().Where(t => t.ResultCode > 0 && t.TradeCode == TradeCodeHelper.TC1005011107 && t.TransactionTime >= today).SumAsync(t => t.Amount);
                    this.RemainWithdrawalAmount = bonusAmount > givenBonusAmount ? bonusAmount - givenBonusAmount : 0;
                }
            }

            return this.RemainWithdrawalAmount.Value;
        }

        private static Tuple<DateTime, DailyConfig> todayConfig = new Tuple<DateTime, DailyConfig>(DateTime.MinValue, null);

        private long? RemainWithdrawalAmount { get; set; }

        private static DailyConfig GetTodayConfig()
        {
            if (todayConfig.Item1 < DateTime.UtcNow.AddHours(8).Date.AddMinutes(20) || todayConfig.Item1 < DateTime.UtcNow.AddHours(8).AddMinutes(-10))
            {
                DailyConfig config = DailyConfigHelper.GetTodayDailyConfig();

                todayConfig = new Tuple<DateTime, DailyConfig>(DateTime.UtcNow.AddHours(8), config);
            }

            return todayConfig.Item2;
        }

        #endregion IBonusManager Members
    }
}