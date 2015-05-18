// FileInformation: nyanya/Cqrs.Domain.Product/ProductService.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/08/27   5:23 PM

using Cat.Commands.Products;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.Services.DTO;
using Cat.Domain.Products.Services.Interfaces;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Cat.Domain.Products.Services
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

        public async Task<CanUpdateShareCountResult> CanUpdateShareCountAsync(string productNo)
        {
            ZCBProduct product;
            using (ProductContext context = new ProductContext())
            {
                product = await context.ReadonlyQuery<ZCBProduct>().Include(o => o.SalePeriod).FirstOrDefaultAsync(p => p.ProductNo == productNo);
            }
            if (product == null)
                return new CanUpdateShareCountResult { Result = true };

            bool canUpdateShareCount = Product.CanUpdateShareCount(product);
            return canUpdateShareCount ? new CanUpdateShareCountResult { ProductIdentifier = product.ProductIdentifier, ProductNo = product.ProductNo, Result = true }
                : new CanUpdateShareCountResult { Result = false, ProductIdentifier = product.ProductIdentifier, ProductNo = product.ProductNo };
        }

        public async Task SetTotalSaleAmount(string productIdentifier, decimal totalSaleAmount)
        {
            await new Product(productIdentifier).SetTotalSaleAmount(totalSaleAmount);
        }

        public async Task SetTotalInterest(string productIdentifier, decimal totalInterest)
        {
            await new Product(productIdentifier).SetTotalInterest(totalInterest);
        }

        public async Task SetTotalRedeemAmount(string productIdentifier, decimal totalRedeemAmount)
        {
            await new Product(productIdentifier).SetTotalRedeemAmount(totalRedeemAmount);
        }

        public async Task SetTotalRedeemInterest(string productIdentifier, decimal totalRedeemInterest)
        {
            await new Product(productIdentifier).SetTotalRedeemInterest(totalRedeemInterest);
        }

        public async Task SetPerRemainRedeemAmount(string productIdentifier, decimal totalRedeemAmount, decimal todayRedeemAmount)
        {
            await new Product(productIdentifier).SetPerRemainRedeemAmount(totalRedeemAmount, todayRedeemAmount);
        }

        public async Task<bool> CanLessPerRemainRedeemAmount(string productNo, decimal redeemAmount)
        {
            ZCBProduct product;
            var result = false;
            using (ProductContext context = new ProductContext())
            {
                using (var tr = context.Database.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    try
                    {
                        product = await context.Query<ZCBProduct>().FirstOrDefaultAsync(p => p.ProductNo == productNo);
                        if (redeemAmount > product.PerRemainRedeemAmount)
                        {
                            tr.Commit();
                        }
                        product.PerRemainRedeemAmount = product.PerRemainRedeemAmount - redeemAmount;
                        await context.SaveChangesAsync();
                        tr.Commit();
                        result = true;
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///     检查产品编号是否已经存在
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public async Task<bool> CheckProductNoExists(string productNo)
        {
            using (ProductContext context = new ProductContext())
            {
                Product product = await context.ReadonlyQuery<Product>().FirstOrDefaultAsync(p => p.ProductNo == productNo);

                if (product != null)
                {
                    return true;
                }
            }
            return false;
        }

        
        #endregion IProductService Members
    }
}