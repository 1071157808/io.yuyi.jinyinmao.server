// ***********************************************************************
// Project          : nyanya
// Author           : Siqi Lu
// Created          : 2015-05-18  2:53 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-19  10:17 AM
// ***********************************************************************
// <copyright file="ProductInfoServiceBase.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Commands.Products;
using Cat.Domain.Products.Database;
using Cat.Domain.Products.Models;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services.DTO;
using Domian.DTO;
using Infrastructure.Cache.Couchbase;

namespace Cat.Domain.Products.Services
{
    public class ProductInfoServiceBase<T> where T : ProductInfo
    {
        protected Func<ProductContext> ProductContextFactory
        {
            get { return () => new ProductContext(); }
        }

        public async Task<IList<ProductWithSaleInfo<T>>> GetBeingSoldOutProductWithSaleInfosAsync()
        {
            IList<T> products = await this.GetOnSaleProductInfosAsync();
            IList<ProductWithSaleInfo<T>> infos = this.BuildProductWithSaleInfos(products.Where(i => !i.SoldOut).ToList());
            return infos.Where(Product.ShouldBeSetSoldOut).ToList();
        }

        public async Task<IList<T>> GetOnSaleProductInfosAsync()
        {
            using (ProductContext context = this.ProductContextFactory.Invoke())
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
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<T>().FirstOrDefaultAsync(p => p.ProductIdentifier == productIdentifier);
            }
        }

        public async Task<T> GetProductInfoByNoAsync(string productNo)
        {
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<T>().FirstOrDefaultAsync(p => p.ProductNo == productNo);
            }
        }

        public async Task<IPaginatedDto<T>> GetProductInfosAsync(int pageIndex, int pageSize = 10, ProductCategory productCategory = ProductCategory.JINYINMAO)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            int totalCount;
            IList<T> items;
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                totalCount = await context.ReadonlyQuery<T>().Where(x => x.ProductCategory == productCategory).CountAsync();
                items = await this.GetSortedProductContext(context).Where(x => x.ProductCategory == productCategory).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
            }
            return new PaginatedDto<T>(pageIndex, pageSize, totalCount, items);
        }

        public async Task<ProductWithSaleInfo<T>> GetProductWithSaleInfoByIdentifierAsync(string productIdentifier)
        {
            T productInfo = await this.GetProductInfoByIdentifierAsync(productIdentifier);
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public virtual async Task<ProductWithSaleInfo<T>> GetProductWithSaleInfoByNoAsync(string productNo)
        {
            T productInfo = await this.GetProductInfoByNoAsync(productNo);
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public virtual async Task<IPaginatedDto<ProductWithSaleInfo<T>>> GetProductWithSaleInfosAsync(int pageIndex, ProductCategory productCategory = ProductCategory.JINYINMAO, int pageSize = 10)
        {
            IPaginatedDto<T> dtos = await this.GetProductInfosAsync(pageIndex, pageSize, productCategory);
            IPaginatedDto<ProductWithSaleInfo<T>> items = this.BuildProductWithSaleInfos(dtos);
            return items;
        }

        public async Task<T> GetTopProductInfoAsync(ProductCategory productCategory = ProductCategory.JINYINMAO)
        {
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                return await this.GetSortedProductContext(context).Where(x => x.ProductCategory == productCategory).FirstOrDefaultAsync();
            }
        }

        public async Task<IList<T>> GetTopProductInfosAsync(int topPageCount = 6, ProductCategory productCategory = ProductCategory.JINYINMAO)
        {
            using (ProductContext context = this.ProductContextFactory.Invoke())
            {
                return await this.GetSortedProductContext(context).Where(x => x.ProductCategory == productCategory).Take(topPageCount).ToListAsync();
            }
        }

        public async Task<ProductWithSaleInfo<T>> GetTopProductWithSaleInfoAsync(ProductCategory productCategory = ProductCategory.JINYINMAO)
        {
            T productInfo = await this.GetTopProductInfoAsync(productCategory);
            return this.BuildProductWithSaleInfo(productInfo);
        }

        public virtual async Task<IList<ProductWithSaleInfo<T>>> GetTopProductWithSaleInfosAsync(int topPageCount = 6, ProductCategory productCategory = ProductCategory.JINYINMAO)
        {
            IList<T> infos = await this.GetTopProductInfosAsync(topPageCount, productCategory);
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

        private IQueryable<T> GetSortedProductContext(ProductContext context)
        {
            DateTime time = DateTime.Now;
            return context.ReadonlyQuery<T>().OrderBy(p => p.SoldOut) // 未售罄 => 0, 售罄 =>1.  => 未售罄的排在前面
                .ThenBy(p => p.EndSellTime < time) // 未停售 => 0, 已停售 => 1. => 未停售的排在前面
                //.ThenBy(p => p.RepaymentDeadline < time) // 未最终还款 => 0, 已还款 => 1. => 售罄的排在结束的之前
                //.ThenBy(p => p.StartSellTime) // 对于未开售的产品，开售时间越小，越早开售
                .ThenByDescending(p => p.LaunchTime); // 后上线的产品排前面
        }
    }
}