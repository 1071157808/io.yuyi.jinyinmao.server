// FileInformation: nyanya/Cat.Domain.Orders/SettlingOrder.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:27 PM

using System;
using Cat.Commands.Orders;
using Domian.Models;
using Cat.Commands.Products;

namespace Cat.Domain.Orders.ReadModels
{
    public class SettlingOrder : IReadModel
    {
        public decimal ExtraInterest { get; set; }

        public decimal Interest { get; set; }

        public OrderType OrderType { get; set; }

        public decimal Principal { get; set; }

        public string ProductIdentifier { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int ProductNumber { get; set; }

        public DateTime SettleDate { get; set; }

        public string UserIdentifier { get; set; }

        /// <summary>
        /// 产品分类 （10金银猫产品 20富滇产品）
        /// </summary>
        public ProductCategory ProductCategory { get; set; }
    }
}