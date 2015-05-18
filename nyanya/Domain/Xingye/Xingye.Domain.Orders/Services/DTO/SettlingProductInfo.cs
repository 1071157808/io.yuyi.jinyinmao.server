// FileInformation: nyanya/Xingye.Domain.Orders/SettlingProductInfo.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:31 PM

using System;
using Xingye.Commands.Products;

namespace Xingye.Domain.Orders.Services.DTO
{
    public class SettlingProductInfo
    {
        /// <summary>
        /// 额外收益
        /// </summary>
        public decimal ExtraInterest { get; set; }
        public decimal Interest { get; set; }

        public decimal Principal { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public ProductType ProductType { get; set; }

        public DateTime SettleDate { get; set; }
    }
}