// FileInformation: nyanya/Cat.Domain.Products/SalePeriod.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using System;
using Domian.Models;

namespace Cat.Domain.Products.Models
{
    public class SalePeriod : IValueObject
    {
        /// <summary>
        ///     停售时间
        /// </summary>
        public DateTime EndSellTime { get; set; }

        public bool OnPreSale
        {
            get
            {
                return this.PreStartSellTime.HasValue && this.PreEndSellTime.HasValue
                       && this.PreStartSellTime.Value < DateTime.Now
                       && this.PreEndSellTime.Value > DateTime.Now.AddSeconds(2);
            }
        }

        public bool OnSale
        {
            get { return this.StartSellTime < DateTime.Now && this.EndSellTime > DateTime.Now.AddSeconds(2); }
        }

        /// <summary>
        ///     提前售卖停止时间
        /// </summary>
        public DateTime? PreEndSellTime { get; set; }

        /// <summary>
        ///     提前售卖开始时间
        /// </summary>
        public DateTime? PreStartSellTime { get; set; }

        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     开售时间
        /// </summary>
        public DateTime StartSellTime { get; set; }
    }
}