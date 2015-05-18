// FileInformation: nyanya/Services.WebAPI.V1.nyanya/InvestmentStatisticDto.cs
// CreatedTime: 2014/04/21   5:10 PM
// LastUpdatedTime: 2014/04/29   2:34 PM

using System;
using Domain.Scheduler.ViewModels;

namespace Services.WebAPI.V1.nyanya.Models.Meow.Statistic
{
    /// <summary>
    ///     InvestmentStatisticDto
    /// </summary>
    public class InvestmentStatisticDto
    {
        /// <summary>
        ///     Gets or sets the defeated percent.
        /// </summary>
        /// <value>
        ///     The defeated percent.
        /// </value>
        public int DefeatedPercent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has orders.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this user has orders; otherwise, <c>false</c>.
        /// </value>
        public bool HasOrders { get; set; }

        /// <summary>
        ///     Gets or sets the interest per second.
        /// </summary>
        /// <value>
        ///     The interest per second.
        /// </value>
        public decimal InterestPerSecond { get; set; }

        /// <summary>
        ///     Gets or sets the time for one cny.  (秒数)
        /// </summary>
        /// <value>
        ///     The time for one cny.
        /// </value>
        public decimal TimeForOneCNY
        {
            get { return this.InterestPerSecond == 0 ? 0 : 1 / this.InterestPerSecond; }
        }
    }

    internal static partial class InvestmentStatisticExtensions
    {
        internal static InvestmentStatisticDto ToInvestmentStatisticDto(this InvestmentStatisticViewModel investmentStatistic)
        {
            return new InvestmentStatisticDto
            {
                DefeatedPercent = Convert.ToInt32(Math.Round(investmentStatistic.PercentRank, MidpointRounding.AwayFromZero)),
                HasOrders = !investmentStatistic.IsOverall,
                InterestPerSecond = investmentStatistic.InterestPerSecond
            };
        }
    }
}