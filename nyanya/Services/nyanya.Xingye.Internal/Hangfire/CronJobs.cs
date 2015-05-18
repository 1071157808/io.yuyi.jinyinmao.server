// FileInformation: nyanya/nyanya.Xingye.Internal/CronJobs.cs
// CreatedTime: 2014/08/28   4:58 PM
// LastUpdatedTime: 2014/08/28   5:18 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xingye.Domain.Products.ReadModels;
using Xingye.Domain.Products.Services;
using Xingye.Domain.Products.Services.DTO;
using Xingye.Domain.Products.Services.Interfaces;
using Xingye.Domain.Yilian.ReadModels;
using Xingye.Domain.Yilian.Services;
using Xingye.Domain.Yilian.Services.Interfaces;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.Lib.Utility;

namespace nyanya.Xingye.Internal.Hangfire
{
    /// <summary>
    ///     CronJobs
    /// </summary>
    public class CronJobs
    {
        private readonly ILogger logger;
        private readonly Lazy<IProductInfoService> productInfoService;
        private readonly Lazy<IProductService> productService;
        private readonly Lazy<IYilianQueryService> yilianQueryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CronJobs"/> class.
        /// </summary>
        public CronJobs()
        {
            this.yilianQueryService = new Lazy<IYilianQueryService>(() => new YilianQueryService());
            this.productInfoService = new Lazy<IProductInfoService>(() => new ProductInfoService());
            this.productService = new Lazy<IProductService>(() => new ProductService());
            this.logger = new NLogger("CronJobsLogger");
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        private ILogger Logger
        {
            get { return this.logger; }
        }

        private IProductInfoService ProductInfoService
        {
            get { return this.productInfoService.Value; }
        }

        private IProductService ProductService
        {
            get { return this.productService.Value; }
        }

        private IYilianQueryService YilianQueryService
        {
            get { return this.yilianQueryService.Value; }
        }

        /// <summary>
        /// Sets the product sold out.
        /// </summary>
        /// <returns></returns>
        public async Task SetProductSoldOut()
        {
            string identifier = GuidUtils.NewGuidString();
            this.Logger.Info("SetProductSoldOut =={0}== => Begin".FmtWith(identifier));

            IList<ProductWithSaleInfo<ProductInfo>> productWithSaleInfoList = await this.ProductInfoService.GetBeingSoldOutProductWithSaleInfosAsync();
            this.Logger.Info("SetProductSoldOut =={0}== => Find BeingSoldOutProducts:{1} ".FmtWith(identifier, productWithSaleInfoList.Count));

            IList<string> productList = productWithSaleInfoList.Select(p => p.ProductInfo.ProductIdentifier).ToList();
            await this.ProductService.SetSoldOut(productList.ToArray());
            foreach (string product in productList)
            {
                this.Logger.Info("SetProductSoldOut =={0}== => {1} SetSaleOut".FmtWith(identifier, product));
            }

            this.Logger.Info("SetProductSoldOut =={0}== => End".FmtWith(identifier));
        }

        /// <summary>
        /// Verifies the order.
        /// </summary>
        /// <returns></returns>
        public async Task VerifyOrder()
        {
            string identifier = GuidUtils.NewGuidString();
            this.Logger.Info("VerifyOrder =={0}== => Begin".FmtWith(identifier));

            IList<YilianQueryView> yilianQueryList = await YilianQueryService.GetUnPaymentListAsync(DateTime.Now.AddMinutes(-3));
            Logger.Info("VerifyOrder =={0}== => Find NoCallbackOrders:{1}".FmtWith(identifier, yilianQueryList.Count));

            await YilianQueryService.QueryYilianPaymentInfo(yilianQueryList);
            foreach (YilianQueryView yilianQueryView in yilianQueryList)
            {
                Logger.Info("VerifyOrder =={0}== => {1} SetOrderResult".FmtWith(identifier, yilianQueryView.OrderIdentifier));
            }

            this.Logger.Info("VerifyOrder =={0}== => End".FmtWith(identifier));
        }

        /// <summary>
        /// Verifies the yilian authentication.
        /// </summary>
        /// <returns></returns>
        public async Task VerifyYilianAuth()
        {
            string identifier = GuidUtils.NewGuidString();
            this.Logger.Info("VerifyYilianAuth( =={0}== => Begin".FmtWith(identifier));
            IList<YilianQueryView> yilianQueryList = await this.YilianQueryService.GetUnCallbackAuthRequestsAsync(DateTime.Now.AddMinutes(-3));
            Logger.Info("VerifyYilianAuth( =={0}== => Find NoCallbackAuthRequests:{1}".FmtWith(identifier, yilianQueryList.Count));
            await this.YilianQueryService.QueryYilian(yilianQueryList);
            foreach (YilianQueryView yilianQueryView in yilianQueryList)
            {
                Logger.Info("VerifyYilianAuth( =={0}== => {1} SetAuthRequestResult".FmtWith(identifier, yilianQueryView.OrderIdentifier));
            }
            this.Logger.Info("VerifyYilianAuth( =={0}== => End".FmtWith(identifier));
        }
    }
}