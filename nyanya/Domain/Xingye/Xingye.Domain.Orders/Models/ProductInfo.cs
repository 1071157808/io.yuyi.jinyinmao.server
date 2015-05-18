// FileInformation: nyanya/Cqrs.Domain.Order/ProductInfo.cs
// CreatedTime: 2014/07/29   10:36 AM
// LastUpdatedTime: 2014/08/04   9:16 AM

using System;

namespace Xingye.Domain.Orders.Models
{
    public class ProductInfo
    {
        public string EndorseImageLink { get; set; }

        public string EndorseImageThumbnailLink { get; set; }

        public string OrderIdentifier { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public decimal ProductYield { get; set; }

        public DateTime RepaymentDeadline { get; set; }

        public decimal UnitPrice { get; set; }
    }
}