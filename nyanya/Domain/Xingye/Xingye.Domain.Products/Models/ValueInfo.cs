// FileInformation: nyanya/Xingye.Domain.Products/ValueInfo.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System;
using Xingye.Commands.Products;

namespace Xingye.Domain.Products.Models
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