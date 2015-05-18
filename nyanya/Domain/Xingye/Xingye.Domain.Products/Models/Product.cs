// FileInformation: nyanya/Xingye.Domain.Products/Product.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System;
using System.Data.Entity;
using System.Linq;
using Domian.Events;
using Domian.Models;
using ServiceStack;
using Xingye.Commands.Products;
using Xingye.Domain.Products.Database;

namespace Xingye.Domain.Products.Models
{
    public partial class Product : IAggregateRoot, IHasMemento
    {
        /// <summary>
        ///     委托协议
        /// </summary>
        public Agreement ConsignmentAgreement { get; set; }

        public int ConsignmentAgreementId { get; set; }

        public DateTime? LaunchTime { get; set; }

        public EndorseLinks Links { get; set; }

        /// <summary>
        ///     项目周期
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        ///     质押借款协议
        /// </summary>
        public Agreement PledgeAgreement { get; set; }

        public int PledgeAgreementId { get; set; }

        /// <summary>
        ///     项目唯一标识
        /// </summary>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     项目名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     项目编号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        ///     项目期数
        /// </summary>
        public int ProductNumber { get; set; }

        /// <summary>
        ///     产品类型
        /// </summary>
        public ProductType ProductType { get; set; }

        public bool Repaid { get; set; }

        /// <summary>
        ///     销售方式信息
        /// </summary>
        public SaleInfo SaleInfo { get; set; }

        /// <summary>
        ///     销售周期信息
        /// </summary>
        public SalePeriod SalePeriod { get; set; }

        /// <summary>
        ///     是否售罄
        /// </summary>
        public bool SoldOut { get; set; }

        /// <summary>
        ///     售罄时间
        /// </summary>
        public DateTime? SoldOutTime { get; set; }

        /// <summary>
        ///     起息信息
        /// </summary>
        public ValueInfo ValueInfo { get; set; }

        /// <summary>
        ///     年化收益
        /// </summary>
        public decimal Yield { get; set; }

        #region IHasMemento Members

        public AggregateMemento GetMemento()
        {
            Product product;
            using (ProductContext context = new ProductContext())
            {
                product = context.ReadonlyQuery<Product>()
                    .Include(p => p.SaleInfo)
                    .Include(p => p.SalePeriod)
                    .Include(p => p.ValueInfo)
                    .FirstOrDefault(p => p.ProductIdentifier == this.ProductIdentifier);
            }

            return new AggregateMemento
            {
                Value = product == null ? "" : product.ToJson()
            };
        }

        #endregion IHasMemento Members
    }
}