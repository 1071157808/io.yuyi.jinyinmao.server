// FileInformation: nyanya/Services.WebAPI.V1.nyanya/InvestmentStatisticSummaryDto.cs
// CreatedTime: 2014/04/21   1:59 PM
// LastUpdatedTime: 2014/05/07   5:09 PM

using System;
using Domain.Scheduler.ViewModels;

namespace Services.WebAPI.V1.nyanya.Models.Meow.Statistic
{
    /// <summary>
    ///     InvestmentStatisticSummaryDto
    /// </summary>
    public class InvestmentStatisticSummaryDto
    {
        /// <summary>
        ///     Gets or sets the accrued earnings.
        /// </summary>
        /// <value>
        ///     The accrued earnings.
        /// </value>
        public decimal AccruedEarnings { get; set; }

        /// <summary>
        ///     Gets the application earning speed. (暂时是每分钟)
        /// </summary>
        /// <value>
        ///     The application earning speed.
        /// </value>
        public decimal AppEearningSpeed
        {
            get { return this.InterestPerSecond * 60; }
        }

        /// <summary>
        ///     Gets or sets the defeated percent.
        /// </summary>
        /// <value>
        ///     The defeated percent.
        /// </value>
        public int DefeatedPercent { get; set; }

        /// <summary>
        ///     Gets or sets the earnings again.
        /// </summary>
        /// <value>
        ///     The earnings again.
        /// </value>
        public decimal EarningsAgain { get; set; }

        /// <summary>
        /// Gets or sets the duration of the earnings again.
        /// </summary>
        /// <value>
        /// The duration of the earnings again.
        /// </value>
        public int EarningsAgainDuration { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has orders.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this user has orders; otherwise, <c>false</c>.
        /// </value>
        public bool HasOrders { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has shown.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has shown; otherwise, <c>false</c>.
        /// </value>
        public bool HasShown { get; set; }

        /// <summary>
        ///     Gets or sets the interest per second.
        /// </summary>
        /// <value>
        ///     The interest per second.
        /// </value>
        public decimal InterestPerSecond { get; set; }
    }

    internal static partial class InvestmentStatisticExtensions
    {
        internal static InvestmentStatisticSummaryDto ToInvestmentStatisticSummaryDto(this InvestmentStatisticSummaryViewModel viewModel)
        {
            return new InvestmentStatisticSummaryDto
            {
                AccruedEarnings = viewModel.AccruedEarnings,
                DefeatedPercent = Convert.ToInt32(Math.Round(viewModel.PercentRank, MidpointRounding.AwayFromZero)),
                EarningsAgain = viewModel.EarningsAgain,
                HasOrders = !viewModel.IsOverall,
                InterestPerSecond = viewModel.InterestPerSecond,
                HasShown = viewModel.HasShown,
                EarningsAgainDuration = viewModel.EarningsAgainDuration
            };
        }
    }
}