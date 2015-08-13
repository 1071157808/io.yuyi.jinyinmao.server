// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : BonusManager.cs
// Created          : 2015-08-13  15:17
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-13  23:08
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
    ///     BonusManager.
    /// </summary>
    public class BonusManager : Grain, IBonusManager
    {
        /// <summary>
        ///     The floor limit
        /// </summary>
        private static readonly int FloorLimit = 1;

        /// <summary>
        ///     The maximum bonus amount
        /// </summary>
        private static readonly long MaxBonusAmount = 1000L;

        /// <summary>
        ///     The minimum bonus amount
        /// </summary>
        private static readonly long MinBonusAmount = 1L;

        /// <summary>
        ///     The random
        /// </summary>
        private static readonly Random Random = new Random(DateTime.Now.DayOfYear);

        /// <summary>
        ///     The upper limit
        /// </summary>
        private static readonly int UpperLimit = 1000;

        private static Tuple<DateTime, DailyConfig> todayConfig = new Tuple<DateTime, DailyConfig>(DateTime.MinValue, null);
        private long? RemainWithdrawalAmount { get; set; }

        #region IBonusManager Members

        /// <summary>
        ///     Gets the bonus amount.
        /// </summary>
        /// <param name="baseAmount">The base amount.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<long> GetBonusAmount(long baseAmount)
        {
            if (await this.GetRemainBonusAmount() <= 0)
            {
                return 1L;
            }

            int r = Random.Next(FloorLimit, UpperLimit + 1);

            long bonus = Convert.ToInt64(Math.Pow(Convert.ToDouble(baseAmount) / 100000000d, 0.25d) * Math.Pow(Convert.ToDouble(r), 0.5d) / 1800);

            if (bonus <= MinBonusAmount)
            {
                bonus = MinBonusAmount;
            }
            else if (bonus > MaxBonusAmount)
            {
                bonus = MaxBonusAmount;
            }

            return bonus;
        }

        #endregion IBonusManager Members

        private static DailyConfig GetTodayConfig()
        {
            if (todayConfig.Item1 < DateTime.UtcNow.AddHours(8).Date.AddMinutes(20) || todayConfig.Item1 < DateTime.UtcNow.AddHours(8).AddMinutes(-10))
            {
                DailyConfig config = DailyConfigHelper.GetTodayDailyConfig();

                todayConfig = new Tuple<DateTime, DailyConfig>(DateTime.UtcNow.AddHours(8), config);
            }

            return todayConfig.Item2;
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
    }
}