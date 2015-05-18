// FileInformation: nyanya/Domain.Meow/OrderStatistic.cs
// CreatedTime: 2014/04/21   1:13 PM
// LastUpdatedTime: 2014/05/04   2:40 PM

using System;
using Domain.Order.Models;

namespace Domain.Meow.Models
{
    public class OrderStatistic
    {
        public virtual int Duration { get; set; }

        public virtual Nullable<decimal> ExtraInterest { get; set; }

        public virtual decimal Interest { get; set; }

        public virtual DateTime InterestAccruingBeginningDay { get; set; }

        public virtual int OrderId { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual DateTime OrderTime { get; set; }

        public virtual decimal Principal { get; set; }

        public virtual int ProductStatus { get; set; }

        public virtual DateTime SettleDay { get; set; }

        public virtual string UserGuid { get; set; }

        public virtual int UserId { get; set; }

        public virtual decimal Yield { get; set; }

        public decimal GetAccruedEarnings()
        {
            if (DateTime.Now >= this.SettleDay.Date)
            {
                return this.Interest;
            }

            return this.GetInterestPerSecond() * Convert.ToDecimal((DateTime.Now - this.InterestAccruingBeginningDay.Date).TotalSeconds);
        }

        public decimal GetAccruedEarningsWithExtraInterest()
        {
            if (DateTime.Now >= this.SettleDay.Date)
            {
                return this.Interest + this.ExtraInterest.GetValueOrDefault();
            }

            return this.GetInterestPerSecondWithExtraInterest() * Convert.ToDecimal((DateTime.Now - this.InterestAccruingBeginningDay.Date).TotalSeconds);
        }

        public decimal GetExtraInterest()
        {
            return this.ExtraInterest.GetValueOrDefault();
        }

        public decimal GetInterestPerSecond()
        {
            if (DateTime.Now >= this.SettleDay.Date)
            {
                return 0;
            }

            return this.Principal * this.Yield / (360 * 24 * 3600);
        }

        public decimal GetInterestPerSecondWithExtraInterest()
        {
            if (DateTime.Now >= this.SettleDay.Date)
            {
                return 0;
            }

            return (this.Interest + this.GetExtraInterest()) / (this.Duration * 24 * 3600);
        }

        public decimal GetInterestWithExtraInterest()
        {
            return this.Interest + this.GetExtraInterest();
        }
    }
}