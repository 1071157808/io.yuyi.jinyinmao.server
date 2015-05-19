using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cat.Commands.Products;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Cat.Domain.Products.Services.Interfaces;
using Domian.DTO;
using Infrastructure.Lib.Utility;

namespace Cat.Domain.Products.Services
{
    public class ZCBProductInfoService : ProductInfoServiceBase<ZCBProductInfo>, IExactZCBProductInfoService
    {
        /// <summary>
        /// 检查该产品是否已下架
        /// </summary>
        /// <param name="productIdentifier"></param>
        /// <returns></returns>
        public async Task<int> CheckEnableSaleZcbProduct(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                string sql = string.Format("Select EnableSale from ZcbProducts where ProductIdentifier = '{0}'", productIdentifier);
                return await context.Database.SqlQuery<int>(sql).FirstAsync();
            }
        }

        public override async Task<ProductWithSaleInfo<ZCBProductInfo>> GetProductWithSaleInfoByNoAsync(string productNo)
        {
            ProductWithSaleInfo<ZCBProductInfo> product = await base.GetProductWithSaleInfoByNoAsync(productNo);

            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                product.ProductInfo.ProductNumber = await context.ReadonlyQuery<ZCBHistory>()
                    .CountAsync(p => p.EnableSale == 1);
            }

            return product;
        }

        public override async Task<IPaginatedDto<ProductWithSaleInfo<ZCBProductInfo>>> GetProductWithSaleInfosAsync(int pageIndex, ProductCategory productCategory = ProductCategory.JINYINMAO, int pageSize = 10)
        {
            var infos = await base.GetProductWithSaleInfosAsync(pageIndex, productCategory, pageSize);

            int number;
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                number = await context.ReadonlyQuery<ZCBHistory>()
                    .CountAsync(p => p.EnableSale == 1);
            }

            foreach (var info in infos.Items)
            {
                info.ProductInfo.ProductNumber = number;
            }

            return infos;
        }

        /// <summary>
        ///     获取赎回本金所需参数
        /// </summary>
        /// <param name="productNo">产品编号</param>
        /// <returns></returns>
        public async Task<RedeemAmountModel> GetRedeemPrincipal(string productNo)
        {
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                var zcbProductInfo = await context.ReadonlyQuery<ZCBProductInfo>()
                    .Where(p => p.ProductNo == productNo).FirstOrDefaultAsync();
                if (zcbProductInfo == null)
                    return null;

                return new RedeemAmountModel
                {
                    PerRemainRedeemAmount = zcbProductInfo.PerRemainRedeemAmount,
                    TotalSaleAmount = zcbProductInfo.TotalSaleAmount
                };
            }
        }

        public async Task<IList<string>> GetSaleProductIdentifierListAsync()
        {
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<ZCBProductInfo>()
                            .Where(p => p.EnableSale == 1)
                            .Select(x => x.ProductIdentifier)
                            .ToListAsync();
            }
        }

        public override async Task<IList<ProductWithSaleInfo<ZCBProductInfo>>> GetTopProductWithSaleInfosAsync(int topPageCount = 6, ProductCategory productCategory = ProductCategory.JINYINMAO)
        {
            IList<ProductWithSaleInfo<ZCBProductInfo>> products = await base.GetTopProductWithSaleInfosAsync(topPageCount, productCategory);
            int number;
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                number = await context.ReadonlyQuery<ZCBHistory>()
                    .CountAsync(p => p.EnableSale == 1);
            }

            foreach (var info in products)
            {
                info.ProductInfo.ProductNumber = number;
            }

            return products;
        }

        public async Task<IList<ZCBHistory>> GetZcbHistorys(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<ZCBHistory>().Where(x => x.ProductIdentifier == productIdentifier).ToListAsync();
            }
        }

        public async Task<IList<ZCBProduct>> GetZcbProductList()
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<ZCBProduct>().ToListAsync();
            }
        }

        public async Task<decimal> GetZcbProductYesterDayYield(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                var sql = string.Format("select top 1 BeforeYield from FE_Products.dbo.ZCBYieldHistory where ProductIdentifier = '{0}' and CONVERT(varchar(50),CreateTime,23) <= '{1}' order by CreateTime desc", productIdentifier, DateTime.Now.ToString("yyyy-MM-dd"));
                var result = await context.Database.SqlQuery<decimal>(sql).ToListAsync();
                return result.Count > 0 ? result.First() : 0;
            }
        }

        /// <summary>
        /// 下架产品
        /// </summary>
        /// <param name="productIdentifier"></param>
        /// <returns></returns>
        public async Task UnShelvesZcbProduct(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                await context.Database.ExecuteSqlCommandAsync("UPDATE ZcbProducts SET EnableSale = 0 WHERE ProductIdentifier = @productIdentifier", new SqlParameter("@productIdentifier", productIdentifier));
            }
        }
    }
}