// FileInformation: nyanya/Domain.Meow/InvestmentStatistic.cs
// CreatedTime: 2014/04/21   1:18 PM
// LastUpdatedTime: 2014/04/28   1:05 PM

using System;

namespace Domain.Scheduler.Models
{
    public class InvestmentStatistic
    {
        public virtual decimal AccruedEarnings { get; set; }

        public virtual int Id { get; set; }

        public virtual decimal InterestPerSecond { get; set; }

        public virtual int OrderCount { get; set; }

        public virtual DateTime UpdateDate { get; set; }

        public virtual long UpdateTime { get; set; }

        public virtual int UserId { get; set; }

        public virtual string UserIdentifier { get; set; }

        public decimal GetCurrentAccruedEarnings()
        {
            return this.AccruedEarnings + this.InterestPerSecond * Convert.ToDecimal((DateTime.Now.AddSeconds(2) - this.UpdateDate).TotalSeconds);
        }
    }
}