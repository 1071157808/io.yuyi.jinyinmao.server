// FileInformation: nyanya/Domain.Amp/ProductService.cs
// CreatedTime: 2014/03/30   9:51 PM
// LastUpdatedTime: 2014/05/07   2:40 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Amp.Models;
using Infrastructure.Data.EntityFramework.Extensions;

namespace Domain.Amp.Services
{
    public class ProductService : IProductService
    {
        // 产品状态备忘：
        // 产品状态 STATUS ||= {'新建' => 10, '已上线' => 20, '下架' => 30, '募集结束' => 40}
        // 募集状态 RAISE_STATUS = {'未募集结束' => 10, '募集失败' => 20, '募集成功' => 30, '开始起息' => 40, '已结息' => 50, '已退款' => 60}

        #region IProductService Members

        public async Task<PaginatedList<Product>> GetSummaryProductsAsync(int pageIndex, int pageSize)
        {
            using (AmpContext context = new AmpContext())
            {
                return await context.Products.AsNoTracking().OrderBy(p => p.SalesStatus). //业务排序：在售产品排在募集结束产品之前
                    ThenBy(p => p.RaiseStatus). //业务排序：'未募集结束' => '募集成功' => '开始起息' =>'已结息'
                    ThenByDescending(p => p.PubBegin). //业务排序：按发售时间倒序
                    ToPaginatedListAsync(pageIndex, pageSize);
            }
        }

        public async Task<IList<TopProduct>> SelectTopProductsAsync(int number = 3)
        {
            IList<TopProduct> products;
            using (AmpContext context = new AmpContext())
            {
                products = await context.TopProducts.AsNoTracking().Take(number).ToListAsync();
            }
            if (products.Count > 0)
            {
                // ReSharper disable once UnusedVariable
                Task t = this.UpdateRecommandedProductAsync(products.First().Id);
            }
            return products;
        }

        #endregion IProductService Members

        private async Task UpdateRecommandedProductAsync(int id)
        {
            using (AmpContext context = new AmpContext())
            {
                MeowConfiguration config = await context.MeowConfigurations.SingleAsync(c => c.Key == "BestProductId");
                if (config.Value != id.ToString())
                {
                    config.Value = id.ToString();
                    config.Description = "Updated automatically at " + DateTime.Now;
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}