// FileInformation: nyanya/QuartzNet/ProductSaleOutJob.cs
// CreatedTime: 2014/08/12   11:47 AM
// LastUpdatedTime: 2014/08/13   7:33 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cqrs.Domain.Products.ReadModels;
using Cqrs.Domain.Products.Services;
using Cqrs.Domain.Products.Services.DTO;
using Cqrs.Domain.Products.Services.Interfaces;
using Quartz;

namespace QuartzNet.Jobs
{
    /// <summary>
    /// </summary>
    public class ProductSaleOutJob : DefaultJob, IProductSaleOutJob
    {
        #region Private Fields

        private readonly IProductInfoService productInfoService;
        private readonly IProductService productService;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// </summary>
        public ProductSaleOutJob()
        {
            this.productInfoService = new ProductInfoService();
            this.productService = new ProductService();
            this.waitTime = new TimeSpan(0, 0, 15, 0);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Jobs the execute.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task JobExecute(IJobExecutionContext context)
        {
            IList<ProductWithSaleInfo<ProductInfo>> productWithSaleInfoList =
                await this.productInfoService.GetBeingSoldOutProductWithSaleInfosAsync();
            if (context != null)
            {
                Logger.Info(DateTime.Now + " Find BeingSoldOutProducts : " + productWithSaleInfoList.Count + " jobkey ； "+context.JobDetail.Key);
            }
            IEnumerable<string> productList =
                productWithSaleInfoList.Select(p => p.ProductInfo.ProductIdentifier);
            IEnumerable<string> enumerable = productList as IList<string> ?? productList.ToList();
            await this.productService.SetSoldOut(enumerable.ToArray());
            foreach (string product in enumerable)
            {
                Logger.Info(DateTime.Now + " try to set saleout , productId : " + product);
            }
        }

        #endregion Public Methods
    }
}