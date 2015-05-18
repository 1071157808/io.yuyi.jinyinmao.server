using System;

namespace Domain.Scheduler.ViewModels
{
    public class InvestmentStatisticHistoryViewModel
    {
        /// <summary>
        /// Gets or sets the last earnings.
        /// </summary>
        /// <value>
        /// The last earnings.
        /// </value>
        public decimal LastEarnings { get; set; }

        /// <summary>
        /// Gets or sets the last showing time.
        /// </summary>
        /// <value>
        /// The last showing time.
        /// </value>
        public DateTime LastShowingTime { get; set; }
    }
}