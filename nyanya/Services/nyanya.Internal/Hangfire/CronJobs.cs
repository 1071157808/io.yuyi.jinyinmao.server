// FileInformation: nyanya/nyanya.Internal/CronJobs.cs
// CreatedTime: 2014/08/28   4:58 PM
// LastUpdatedTime: 2014/08/28   5:18 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Orders.Services;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Products.ReadModels;
using Cat.Domain.Products.Services;
using Cat.Domain.Products.Services.DTO;
using Cat.Domain.Products.Services.Interfaces;
using Cat.Domain.Yilian.ReadModels;
using Cat.Domain.Yilian.Services;
using Cat.Domain.Yilian.Services.Interfaces;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.Lib.Utility;
using Moe.Lib;

namespace nyanya.Internal.Hangfire
{
    /// <summary>
    ///     CronJobs
    /// </summary>
    public class CronJobs
    {
        #region 构造

        private readonly ILogger logger;
        private readonly Lazy<IOrderInfoService> orderInfoService;
        private readonly Lazy<IOrderService> orderService;
        private readonly Lazy<IProductInfoService> productInfoService;
        private readonly Lazy<IProductService> productService;
        private readonly Lazy<IYilianQueryService> yilianQueryService;
        private readonly Lazy<IZCBOrderService> zcbOrderService;
        private readonly Lazy<IZCBProductInfoService> zcbProductService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CronJobs"/> class.
        /// </summary>
        public CronJobs()
        {
            this.yilianQueryService = new Lazy<IYilianQueryService>(() => new YilianQueryService());
            this.productInfoService = new Lazy<IProductInfoService>(() => new ProductInfoService());
            this.orderInfoService = new Lazy<IOrderInfoService>(() => new OrderInfoService());
            this.productService = new Lazy<IProductService>(() => new ProductService());
            this.zcbProductService = new Lazy<IZCBProductInfoService>(() => new ZCBProductInfoService());
            this.zcbOrderService = new Lazy<IZCBOrderService>(() => new ZCBOrderService());
            this.orderService = new Lazy<IOrderService>(() => new OrderService());
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

        private IOrderInfoService OrderInfoService
        {
            get { return this.orderInfoService.Value; }
        }

        private IOrderService OrderService
        {
            get { return this.orderService.Value; }
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

        private IZCBOrderService ZcbOrderService
        {
            get { return zcbOrderService.Value; }
        }

        private IZCBProductInfoService ZcbProductInfoService
        {
            get { return this.zcbProductService.Value; }
        }

        #endregion 构造

        #region 任务

        /// <summary>
        /// 产品统计信息（累计售出、累计收益、累计赎回、当日剩余取款金额、更新资产包用户收益）（今天之前）
        /// </summary>
        /// <returns></returns>
        public async Task ProductStatistics()
        {
            string identifier = GuidUtils.NewGuidString();
            Logger.Info("ProductStatistics({0}) => Begin".FmtWith(identifier));

            try
            {
                //更新资产包用户收益
                await SetZcbUserInterest(identifier);
                //更新产品累计售出、累计收益、累计赎回、当日剩余取款金额
                await UpdateZcbPorduct(identifier);
            }
            catch (Exception e)
            {
                Logger.Error("ProductStatistics({0}) => {1}".FmtWith(identifier, e.GetExceptionString()));
            }

            Logger.Info("ProductStatistics({0}) => End".FmtWith(identifier));
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

            IList<string> productList = productWithSaleInfoList.Where(p => p.ProductInfo.ProductType != global::Cat.Commands.Products.ProductType.ZCBAcceptance).Select(p => p.ProductInfo.ProductIdentifier).ToList();
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

            IList<YilianQueryView> yilianQueryList = await YilianQueryService.GetUnPaymentResultAsync(DateTime.Now.AddMinutes(-3), await OrderInfoService.GetUnResultOrdersAsync(DateTime.Now.AddMinutes(-3)));
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

        #endregion 任务

        #region 私有方法

        /// <summary>
        /// 更新资产包用户收益
        /// </summary>
        /// <returns></returns>
        private async Task SetZcbUserInterest(string identifier)
        {
            //当前再投金额大于0的用户
            var users = await ZcbOrderService.GetActiveZcbUsers();
            Logger.Info("ProductStatistics({0}) => SetZcbUserInterest FindCount:{1}".FmtWith(identifier, users.Count));
            foreach (var zcbUser in users)
            {
                Logger.Info("ProductStatistics({0}) => SetZcbUserInterest User: {1} Begin".FmtWith(identifier, zcbUser.UserIdentifier));
                //产品收益率历史记录
                var zcbYieldHistorys = await ZcbProductInfoService.GetZcbHistorys(zcbUser.ProductIdentifier);
                if (zcbYieldHistorys == null || zcbYieldHistorys.Count == 0) continue;
                //添加昨日收益、更新昨日收益、更新总收益
                await ZcbOrderService.SetYesterDayInterest(zcbUser, zcbYieldHistorys);
                Logger.Info("ProductStatistics({0}) => SetZcbUserInterest User: {1} SetYesterDayInterest Finsh!".FmtWith(identifier, zcbUser.UserIdentifier));
                //检查提现申请
                await ZcbOrderService.CheckRedeemApplication(zcbUser, zcbYieldHistorys);
                Logger.Info("ProductStatistics({0}) => SetZcbUserInterest User: {1} End".FmtWith(identifier, zcbUser.UserIdentifier));
            }

            Logger.Info("ProductStatistics({0}) => SetZcbUserInterest Finish!".FmtWith(identifier));
        }

        /// <summary>
        /// 更新产品累计售出、累计收益、累计赎回、当日剩余取款金额（今天之前）
        /// </summary>
        /// <returns></returns>
        private async Task UpdateZcbPorduct(string identifier)
        {
            var productIdentifierList = await ZcbProductInfoService.GetSaleProductIdentifierListAsync();
            Logger.Info("ProductStatistics({0}) => UpdateZcbPorduct Find ProductCount:{1}".FmtWith(identifier, productIdentifierList.Count));
            foreach (var proIdentifier in productIdentifierList)
            {
                //累计售出
                var totalSaleAmount = await OrderInfoService.GetTotalSaleAmountByProductAsync(proIdentifier);
                await ProductService.SetTotalSaleAmount(proIdentifier, totalSaleAmount);
                //累计收益
                var totalInterest = await OrderInfoService.GetTotalInterestByProductAsync(proIdentifier);
                await ProductService.SetTotalInterest(proIdentifier, totalInterest);
                //累计在投金额
                var totalCurrentAmount = await OrderInfoService.GetTotalCurrentAmountByProductAsync(proIdentifier);
                //累计赎回
                await ProductService.SetTotalRedeemAmount(proIdentifier, totalSaleAmount - totalCurrentAmount);
                //累计赎回收益
                var totalRedeemInterest = await OrderInfoService.GetTotalRedeemInterestByProductAsync(proIdentifier);
                await ProductService.SetTotalRedeemInterest(proIdentifier, totalRedeemInterest);
            }
            Logger.Info("ProductStatistics({0}) => UpdateZcbPorduct Finish!".FmtWith(identifier));
        }

        #endregion 私有方法
    }
}