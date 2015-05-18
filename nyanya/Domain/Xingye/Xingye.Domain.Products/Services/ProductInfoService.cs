// FileInformation: nyanya/Cqrs.Domain.Product/ProductInfoService.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/15   7:53 PM

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xingye.Domain.Products.Database;
using Xingye.Domain.Products.Models;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.Interfaces;
using Infrastructure.Cache.Couchbase;

namespace Xingye.Domain.Products.Services
{
    public class ProductInfoService : ProductInfoServiceBase<ProductInfo>, IExactProductInfoService
    {
        #region IExactProductInfoService Members

        public async Task<AgreementsPackage> GetAgreementsInfoAsync(string productIdentifier)
        {
            Product product = new Product(productIdentifier);
            return await product.GetAgreementsAsync();
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

        #endregion IExactProductInfoService Members
    }
}