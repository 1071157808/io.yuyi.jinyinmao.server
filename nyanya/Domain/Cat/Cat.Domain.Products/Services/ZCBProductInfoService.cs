
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.Interfaces;
using Cat.Domain.Products.Services.DTO;
using Infrastructure.Lib.Utility;
using Cat.Commands.Products;
using System.Data.SqlClient;

namespace Cat.Domain.Products.Services
{
    public class ZCBProductInfoService : ProductInfoServiceBase<ZCBProductInfo>, IExactZCBProductInfoService
    {
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

        public async Task<IList<ZCBProduct>> GetZcbProductList()
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<ZCBProduct>().ToListAsync();
            }
        }

        public async Task<IList<ZCBHistory>> GetZcbHistorys(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<ZCBHistory>().Where(x => x.ProductIdentifier == productIdentifier).ToListAsync();
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

        /// <summary>
        /// 检查该产品是否已下架
        /// </summary>
        /// <param name="productIdentifier"></param>
        /// <returns></returns>
        public async Task<int> CheckEnableSaleZcbProduct(string productIdentifier)
        {
            using(ProductContext context = new ProductContext())
            {
                string sql = string.Format("Select EnableSale from ZcbProducts where ProductIdentifier = '{0}'", productIdentifier);
                return await context.Database.SqlQuery<int>(sql).FirstAsync();
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
