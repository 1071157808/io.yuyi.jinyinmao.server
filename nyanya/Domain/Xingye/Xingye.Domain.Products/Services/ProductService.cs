// FileInformation: nyanya/Cqrs.Domain.Product/ProductService.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/27   5:23 PM

using System.Data.Entity;
using System.Threading.Tasks;
using Xingye.Domain.Products.Database;
using Xingye.Domain.Products.Models;
using Xingye.Domain.Products.Services.DTO;
using Xingye.Domain.Products.Services.Interfaces;

namespace Xingye.Domain.Products.Services
{
    public class ProductService : IProductService
    {
        #region IProductService Members

        public async Task<CanRepayResult> CanRepayAsync(string productNo)
        {
            Product product;
            using (ProductContext context = new ProductContext())
            {
                product = await context.ReadonlyQuery<Product>().Include(p => p.SalePeriod).FirstOrDefaultAsync(p => p.ProductNo == productNo);
            }

            bool canRepay = product != null;

            return canRepay ? new CanRepayResult { ProductIdentifier = product.ProductIdentifier, ProductNo = product.ProductNo, Result = true }
                : new CanRepayResult { Result = false };
        }

        public async Task<CanUnShelvesResult> CanUnShelvesAsync(string productNo)
        {
            Product product;
            using (ProductContext context = new ProductContext())
            {
                product = await context.ReadonlyQuery<BankAcceptanceProduct>().Include(p => p.SalePeriod).FirstOrDefaultAsync(p => p.ProductNo == productNo);
            }

            bool canUnShelves = Product.CanUnShelves(product);

            return canUnShelves ? new CanUnShelvesResult { ProductIdentifier = product.ProductIdentifier, ProductNo = product.ProductNo, Result = true }
                : new CanUnShelvesResult { Result = false };
        }

        public bool FreezeShareCount(string productIdentifier, int count)
        {
            Product product = new Product(productIdentifier);
            return product.FreezeShareCount(count);
        }

        public async Task SetSoldOut(params string[] productIdentifier)
        {
            foreach (string s in productIdentifier)
            {
                Product product = new Product(s);
                await product.SetSoldOutAsync();
            }
        }

        public bool UnfreezeShareCount(string productIdentifier, int count)
        {
            Product product = new Product(productIdentifier);
            return product.UnfreezeShareCount(count);
        }

        public async Task UnShelvesAsync(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                Product product = await context.Query<Product>().Include(p => p.SalePeriod).FirstOrDefaultAsync(p => p.ProductIdentifier == productIdentifier);

                if (product != null)
                {
                    await product.UnShelvesAsync();
                }
            }
        }

        #endregion IProductService Members
    }
}