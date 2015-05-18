using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cqrs.Domain.Products.Database;
using Cqrs.Domain.Products.ReadModels;
using Cqrs.Domain.Products.Services.DTO;
using Cqrs.Domain.Products.Services.Interfaces;
using Infrastructure.Cache.Couchbase;

namespace Cqrs.Domain.Products.Services
{
    public class  JymProductInfoService<T, U, V,W> : IJymProductInfoService<T,U,V,W> 
        where T : ProductInfo where U : ProductListModel<T>, new() 
        where V : ProductWithSaleInfo ,new() where W : ProductWithSaleListModel<V>,new()
        
    {
        private const int CommonPageCount = 10;
        private const int IndexPageCount = 6;
        public async Task<T> GetProductInfoByIdentifier(string productIdentifier)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<T>().FirstOrDefaultAsync(p => p.ProductIdentifier == productIdentifier);
            }
        }

        public async Task<T> GetProductInfoByNo(string productNo)
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<T>().FirstOrDefaultAsync(p => p.ProductNo == productNo);
            }
        }

        public async Task<U> GetProductList(int pageNumber)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            int totalPageCount;
            IList<T> items;
            using (ProductContext context = new ProductContext())
            {
                totalPageCount = (await context.ReadonlyQuery<T>().CountAsync() + (CommonPageCount - 1)) / CommonPageCount;
                items = await context.ReadonlyQuery<T>().OrderBy(p => p.SoldOut).ThenBy(p => p.StartSellTime).ThenByDescending(p => p.LaunchTime).Skip(CommonPageCount * (pageNumber - 1)).Take(CommonPageCount).ToListAsync();
            }
            return new U { Items = items, TotalPageCount = totalPageCount };
        }

        public async Task<V> GetProductWithSaleInfoByIdentifier(string productIdentifier)
        {
            T productInfo = await this.GetProductInfoByIdentifier(productIdentifier);
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public async Task<V> GetProductWithSaleInfoByNo(string productNo)
        {
           T productInfo = await this.GetProductInfoByNo(productNo);
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public async Task<W> GetProductWithSaleList(int pageNumber)
        {
           U model = await this.GetProductList(pageNumber);
            IList<V> items = this.BuildProductWithSaleInfos(model.Items);
            return new W { Items = items, TotalPageCount = model.TotalPageCount };
        }

        public async Task<T> GetTopProductInfo()
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<T>().OrderBy(p => p.SoldOut).ThenBy(p => p.StartSellTime).ThenByDescending(p => p.LaunchTime).FirstOrDefaultAsync();
            }
        }

        public async Task<IList<T>> GetTopProductList()
        {
            using (ProductContext context = new ProductContext())
            {
                return await context.ReadonlyQuery<T>().OrderBy(p => p.SoldOut).ThenBy(p => p.StartSellTime).ThenByDescending(p => p.LaunchTime).Take(IndexPageCount).ToListAsync();
            }
        }

        public async Task<V> GetTopProductWithSale()
        {
            T productInfo = await this.GetTopProductInfo();
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public async Task<IList<V>> GetTopProductWithSaleList()
        {
            IList<T> infos = await this.GetTopProductList();
            return this.BuildProductWithSaleInfos(infos);
        }
        private V BuildProductWithSaleInfo(T productInfo)
        {
            if (productInfo == null)
            {
                return null;
            }
            V v = new V();
            if (productInfo.SoldOut)
            {
                
                v.SetProductInfo(productInfo);
                return v;
            }

            ProductShareCacheModel shareModel = ProductShareCache.GetShareCache(productInfo.ProductIdentifier);
            
            v.SetProductInfo(productInfo).SetShareModel(shareModel);
            return v;
            
        }

        private IList<V> BuildProductWithSaleInfos(IList<T> infos)
        {
            IEnumerable<string> identifiers = infos.Where(i => !i.SoldOut).Select(i => i.ProductIdentifier);
            IDictionary<string, ProductShareCacheModel> shareModels = ProductShareCache.GetShareCaches(identifiers);
            List<V> results = new List<V>();
            V v = new V();
            foreach (T info in infos)
            {
                ProductShareCacheModel value;
                if (shareModels.TryGetValue(info.ProductIdentifier, out value))
                {
                    v.SetProductInfo(info).SetShareModel(value);
                }
                else
                {
                    v.SetProductInfo(info);
                }
                results.Add(v);
            }
            return results;
        }
    }
}
