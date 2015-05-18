// FileInformation: nyanya/Cat.Domain.Products/ProductInfoService.cs
// CreatedTime: 2014/09/17   11:18 AM
// LastUpdatedTime: 2014/09/17   12:28 PM

using Cat.Commands.Products;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.Interfaces;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Lib.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Cat.Domain.Products.Services
{
    public class ProductInfoService : ProductInfoServiceBase<ProductInfo>, IExactProductInfoService
    {
        #region IExactProductInfoService Members

        public async Task<AgreementsPackage> GetAgreementsInfoAsync(string productIdentifier)
        {
            Guard.IdentifierMustBeAssigned(productIdentifier, this.GetType().ToString());
            Product product;
            using (ProductContext context = new ProductContext())
            {
                product = await context.ReadonlyQuery<Product>().Include(p => p.PledgeAgreement).Include(p => p.ConsignmentAgreement).FirstAsync(p => p.ProductIdentifier == productIdentifier);
            }

            return product.GetAgreements();
        }

        public async Task<AgreementsPackage> GetZCBPledgeAgreementInfoAsync(string productIdentifier)
        {
            Guard.IdentifierMustBeAssigned(productIdentifier, this.GetType().ToString());
            ZCBProduct product;
            using (ProductContext context = new ProductContext())
            {
                product = await context.ReadonlyQuery<ZCBProduct>().Include(p => p.PledgeAgreement).FirstAsync(p => p.ProductIdentifier == productIdentifier);
            }
            return new AgreementsPackage { PledgeAgreementContent = product.PledgeAgreement.Content, PledgeAgreementName = product.PledgeAgreementName };
        }

        public async Task<string> GetConsignmentAgreementAsync(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<Product>().Where(p => p.ProductIdentifier == productIdentifier)
                    .Select(p => p.ConsignmentAgreement.Content).FirstOrDefaultAsync();
            }
        }

        public async Task<string> GetPledgeAgreementAsync(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<Product>().Where(p => p.ProductIdentifier == productIdentifier)
                    .Select(p => p.PledgeAgreement.Content).FirstOrDefaultAsync();
            }
        }

        public ProductShareCacheModel GetProductSaleProcess(string productIdentifier)
        {
            Product product = new Product(productIdentifier);
            return product.GetShareCache();
        }

        public IDictionary<string, ProductShareCacheModel> GetProductSaleProcess(IEnumerable<string> productIdentifiers)
        {
            return ProductShareCache.GetShareCaches(productIdentifiers);
        }

        public async Task<int> GetRepaidCountAsync()
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.Database.SqlQuery<int>("select count(1) from Products where Repaid=1").FirstAsync();
            }
        }

        public string GetSnapshot(string productIdentifier)
        {
            Product product = new Product(productIdentifier);
            return product.GetMemento().Value;
        }

        public async Task<Product> GetProductByNo(string productNo)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.Query<Product>().FirstOrDefaultAsync(p => p.ProductNo == productNo);
            }
        }

        public async Task<decimal> GetProductYield(string productNo)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<Product>().Where(p => p.ProductNo == productNo)
                    .Select(p => p.Yield).FirstOrDefaultAsync();
            }
        }

        /// <summary>
        ///  获取第一条产品记录
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns></returns>
        public async Task<Product> GetFirstProduct(ProductType productType)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<Product>().FirstOrDefaultAsync(p => p.ProductType == productType);
            }
        }
        #endregion IExactProductInfoService Members
    }
}