// FileInformation: nyanya/Xingye.Domain.Products/ProductInfoServiceBase.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Lib.Data;
using Xingye.Domain.Products.Database;
using Xingye.Domain.Products.Models;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services.DTO;
using Domian.DTO;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Lib.Extensions;

namespace Xingye.Domain.Products.Services
{
    public class ProductInfoServiceBase<T> where T : ProductInfo
    {
        public async Task<IList<ProductWithSaleInfo<T>>> GetBeingSoldOutProductWithSaleInfosAsync()
        {
            IList<T> products = await this.GetOnSaleProductInfosAsync();
            IList<ProductWithSaleInfo<T>> infos = BuildProductWithSaleInfos(products.Where(i => !i.SoldOut).ToList());
            return infos.Where(Product.ShouldBeSetSoldOut).ToList();
        }

        public async Task<IList<T>> GetOnSaleProductInfosAsync()
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<T>().Where(p => !p.SoldOut).ToListAsync();
            }
        }

        public async Task<IList<ProductWithSaleInfo<T>>> GetOnSaleProductWithSaleInfosAsync()
        {
            IList<T> infos = await this.GetOnSaleProductInfosAsync();
            return this.BuildProductWithSaleInfos(infos);
        }

        public async Task<T> GetProductInfoByIdentifierAsync(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<T>().FirstOrDefaultAsync(p => p.ProductIdentifier == productIdentifier);
            }
        }

        public async Task<T> GetProductInfoByNoAsync(string productNo)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<T>().FirstOrDefaultAsync(p => p.ProductNo == productNo);
            }
        }

        public async Task<IPaginatedDto<T>> GetProductInfosAsync(int pageIndex, int pageSize = 10)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            int totalCount;
            IList<T> items;
            using (ProductContext context = new ProductContext())
            {
                totalCount = await context.ReadonlyQuery<T>().CountAsync();
                items = await this.GetSortedProductContext(context).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
            }
            return new PaginatedDto<T>(pageIndex, pageSize, totalCount, items);
        }

        public async Task<ProductWithSaleInfo<T>> GetProductWithSaleInfoByIdentifierAsync(string productIdentifier)
        {
            T productInfo = await this.GetProductInfoByIdentifierAsync(productIdentifier);
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public async Task<ProductWithSaleInfo<T>> GetProductWithSaleInfoByNoAsync(string productNo)
        {
            T productInfo = await this.GetProductInfoByNoAsync(productNo);
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public async Task<IPaginatedDto<ProductWithSaleInfo<T>>> GetProductWithSaleInfosAsync(int pageIndex, int pageSize = 10)
        {
            IPaginatedDto<T> dtos = await this.GetProductInfosAsync(pageIndex, pageSize);
            IPaginatedDto<ProductWithSaleInfo<T>> items = this.BuildProductWithSaleInfos(dtos);
            return items;
        }

        public async Task<IPaginatedDto<ProductWithSaleInfo<T>>> GetProductWithSaleInfosAsync(int pageIndex, SortCondition[] sortConditions, int pageSize = 10)
        {
            IPaginatedDto<T> dtos = await this.GetProductInfosAsync(pageIndex, sortConditions, pageSize);
            IPaginatedDto<ProductWithSaleInfo<T>> items = this.BuildProductWithSaleInfos(dtos);
            return items;
        }

        public async Task<T> GetTopProductInfoAsync()
        {
            using (ProductContext context = new ProductContext())
            {
                return await this.GetSortedProductContext(context).FirstOrDefaultAsync();
            }
        }

        public async Task<IList<T>> GetTopProductInfosAsync(int topPageCount = 6)
        {
            using (ProductContext context = new ProductContext())
            {
                return await this.GetSortedProductContext(context).Take(topPageCount).ToListAsync();
            }
        }

        public async Task<ProductWithSaleInfo<T>> GetTopProductWithSaleInfoAsync()
        {
            T productInfo = await this.GetTopProductInfoAsync();
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public async Task<IList<ProductWithSaleInfo<T>>> GetTopProductWithSaleInfosAsync(int topPageCount = 6)
        {
            IList<T> infos = await this.GetTopProductInfosAsync(topPageCount);
            return this.BuildProductWithSaleInfos(infos);
        }

        private ProductWithSaleInfo<T> BuildProductWithSaleInfo(T productInfo)
        {
            if (productInfo == null)
            {
                return null;
            }

            if (productInfo.SoldOut)
            {
                return new ProductWithSaleInfo<T>(productInfo);
            }

            ProductShareCacheModel shareModel = ProductShareCache.GetShareCache(productInfo.ProductIdentifier);
            return new ProductWithSaleInfo<T>(productInfo, shareModel);
        }

        private IList<ProductWithSaleInfo<T>> BuildProductWithSaleInfos(IList<T> infos)
        {
            IEnumerable<string> identifiers = infos.Where(i => !i.SoldOut).Select(i => i.ProductIdentifier);
            IDictionary<string, ProductShareCacheModel> shareModels = ProductShareCache.GetShareCaches(identifiers);
            List<ProductWithSaleInfo<T>> results = new List<ProductWithSaleInfo<T>>();
            foreach (T info in infos)
            {
                ProductShareCacheModel value;
                results.Add(shareModels.TryGetValue(info.ProductIdentifier, out value) ? new ProductWithSaleInfo<T>(info, value) : new ProductWithSaleInfo<T>(info));
            }
            return results;
        }

        private IPaginatedDto<ProductWithSaleInfo<T>> BuildProductWithSaleInfos(IPaginatedDto<T> infos)
        {
            IList<ProductWithSaleInfo<T>> results = this.BuildProductWithSaleInfos(infos.Items.ToList());
            return infos.ToPaginatedDto(results);
        }

        private async Task<IPaginatedDto<T>> GetProductInfosAsync(int pageIndex, SortCondition[] sortConditions, int pageSize = 10)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            int totalCount;
            IList<T> items;
            using (var context = new ProductContext())
            {
                totalCount = await context.ReadonlyQuery<T>().CountAsync();
                if (sortConditions.Any())
                {
                    items = await this.GetSortedProductContextNoLaunchTime(context).ThenBy(sortConditions).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                }
                else
                {
                    items = await this.GetSortedProductContext(context).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                }
            }
            return new PaginatedDto<T>(pageIndex, pageSize, totalCount, items);
        }

        private IOrderedQueryable<T> GetSortedProductContext(ProductContext context)
        {
            DateTime time = DateTime.Now;
            return context.ReadonlyQuery<T>().OrderBy(p => p.SoldOut) // 未售罄 => 0, 售罄 =>1.  => 未售罄的排在前面
                .ThenBy(p => p.EndSellTime < time) // 未停售 => 0, 已停售 => 1. => 未停售的排在前面
                .ThenBy(p => p.Repaid) // 未还款 => 0, 本息归还 => 1. => 未还款的排前面
            //.ThenBy(p => p.RepaymentDeadline < time) // 未最终还款 => 0, 已还款 => 1. => 售罄的排在结束的之前
            //.ThenBy(p => p.StartSellTime) // 对于未开售的产品，开售时间越小，越早开售
            .ThenByDescending(p => p.LaunchTime); // 后上线的产品排前面
        }

        private IOrderedQueryable<T> GetSortedProductContextNoLaunchTime(ProductContext context)
        {
            DateTime time = DateTime.Now;
            return context.ReadonlyQuery<T>().OrderBy(p => p.SoldOut) // 未售罄 => 0, 售罄 =>1.  => 未售罄的排在前面
                .ThenBy(p => p.EndSellTime < time) // 未停售 => 0, 已停售 => 1. => 未停售的排在前面
                .ThenBy(p => p.Repaid); // 未还款 => 0, 本息归还 => 1. => 未还款的排前面
            //.ThenBy(p => p.RepaymentDeadline < time) // 未最终还款 => 0, 已还款 => 1. => 售罄的排在结束的之前
            //.ThenBy(p => p.StartSellTime) // 对于未开售的产品，开售时间越小，越早开售
            //.ThenByDescending(p => p.LaunchTime); // 后上线的产品排前面
        }
    }
}