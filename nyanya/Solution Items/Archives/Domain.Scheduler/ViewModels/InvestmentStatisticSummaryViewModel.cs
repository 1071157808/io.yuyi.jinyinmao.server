// FileInformation: nyanya/Domain.Meow/InvestmentStatisticSummaryViewModel.cs
// CreatedTime: 2014/04/21 1:28 PM
// LastUpdatedTime: 2014/04/27 6:21 PM

namespace Domain.Scheduler.ViewModels
{
    /// <summary>
    /// InvestmentStatisticSummaryViewModel
    /// </summary>
    public class InvestmentStatisticSummaryViewModel
    {
        /// <summary>
        /// Gets or sets the accrued earnings.
        /// </summary>
        /// <value>The accrued earnings.</value>
        public decimal AccruedEarnings { get; set; }

        /// <summary>
        /// Gets or sets the earnings again.
        /// </summary>
        /// <value>The earnings again.</value>
        public decimal EarningsAgain { get; set; }

        /// <summary>
        /// Gets or sets the duration of the earnings again.
        /// </summary>
        /// <value>The duration of the earnings again.</value>
        public int EarningsAgainDuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has shown.
        /// </summary>
        /// <value><c>true</c> if this instance has shown; otherwise, <c>false</c>.</value>
        public bool HasShown { get; set; }

        /// <summary>
        /// Gets or sets the interest per second.
        /// </summary>
        /// <value>The interest per second.</value>
        public decimal InterestPerSecond { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is overall.
        /// </summary>
        /// <value><c>true</c> if this instance is overall; otherwise, <c>false</c>.</value>
        public bool IsOverall { get; set; }

        /// <summary>
        /// Gets or sets the percent rank.
        /// </summary>
        /// <value>The percent rank.</value>
        public decimal PercentRank { get; set; }
    }
}