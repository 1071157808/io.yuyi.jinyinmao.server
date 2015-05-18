// FileInformation: nyanya/Domain.Scheduler/InvestmentStatisticViewModel.cs
// CreatedTime: 2014/04/28   5:46 PM
// LastUpdatedTime: 2014/04/28   6:12 PM

namespace Domain.Scheduler.ViewModels
{
    public class InvestmentStatisticViewModel
    {
        /// <summary>
        ///     Gets or sets the interest per second.
        /// </summary>
        /// <value>
        ///     The interest per second.
        /// </value>
        public decimal InterestPerSecond { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is overall.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is overall; otherwise, <c>false</c>.
        /// </value>
        public bool IsOverall { get; set; }

        /// <summary>
        ///     Gets or sets the percent rank.
        /// </summary>
        /// <value>
        ///     The percent rank.
        /// </value>
        public decimal PercentRank { get; set; }
    }
}