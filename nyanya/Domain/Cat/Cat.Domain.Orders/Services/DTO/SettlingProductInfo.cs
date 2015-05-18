// FileInformation: nyanya/Cat.Domain.Orders/SettlingProductInfo.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using System;
using Cat.Commands.Products;

namespace Cat.Domain.Orders.Services.DTO
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

        public ProductCategory Category { get; set; }
    }
}