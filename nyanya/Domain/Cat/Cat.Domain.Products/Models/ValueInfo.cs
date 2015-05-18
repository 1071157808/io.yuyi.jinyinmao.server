// FileInformation: nyanya/Cat.Domain.Products/ValueInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:25 PM

using System;
using Cat.Commands.Products;

namespace Cat.Domain.Products.Models
{
    public class ValueInfo
    {
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     最迟还款日
        /// </summary>
        public DateTime RepaymentDeadline { get; set; }

        /// <summary>
        ///     结息日期
        /// </summary>
        public DateTime SettleDate { get; set; }

        /// <summary>
        ///     指定起息时间
        /// </summary>
        public DateTime? ValueDate { get; set; }

        /// <summary>
        ///     起息方式
        /// </summary>
        public ValueDateMode ValueDateMode { get; set; }
    }
}