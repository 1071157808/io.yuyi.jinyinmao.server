// FileInformation: nyanya/nyanya.Cat/MeowController.cs
// CreatedTime: 2014/08/29   2:51 PM
// LastUpdatedTime: 2014/09/09   4:01 PM

using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Products.Services.Interfaces;
using nyanya.Cat.Models;

namespace nyanya.Cat.Controllers
{
    /// <summary>
    ///     Meow 的配置接口
    /// </summary>
    [RoutePrefix("Meow")]
    public class MeowController : ApiController
    {
        private readonly IOrderInfoService orderInfoService;
        private readonly IProductInfoService productInfoService;
        private readonly Xingye.Domain.Orders.Services.Interfaces.IOrderInfoService xyOrderInfoService;
        private readonly Xingye.Domain.Products.Services.Interfaces.IProductInfoService xyProductInfoService;
        /// <summary>
        ///     Initializes a new instance of the <see cref="MeowController" /> class.
        /// </summary>
        /// <param name="orderInfoService">The order information service.</param>
        /// <param name="productInfoService">The product information service.</param>
        public MeowController(IOrderInfoService orderInfoService, IProductInfoService productInfoService,Xingye.Domain.Orders.Services.Interfaces.IOrderInfoService xyOrderInfoService,
            Xingye.Domain.Products.Services.Interfaces.IProductInfoService xyProductInfoService)
        {
            this.orderInfoService = orderInfoService;
            this.productInfoService = productInfoService;
            this.xyOrderInfoService = xyOrderInfoService;
            this.xyProductInfoService = xyProductInfoService;
        }

        /// <summary>
        ///     获取首页的统计信息（合并金银猫、兴业、富滇）
        /// </summary>
        /// <returns>
        ///     OrderCount[int]: 累计购买订单数量
        ///     ProductCount[int]: 累计还款期数
        ///     OutletCount[int]: 线下网店数量
        ///     TotalInterest[decimal]：累计创造总收益
        /// </returns>
        [Route("IndexStatistics")]
        [ResponseType(typeof(IndexStatistics))]
        public async Task<IHttpActionResult> GetIndexStatistics()
        {
            Task<int> orderCountTask = this.orderInfoService.GetPaidCountAsync();
            Task<int> productCountTask = this.productInfoService.GetRepaidCountAsync();
            Task<decimal> totalInterest = this.orderInfoService.GetTotalInterestAsync();
            await Task.WhenAll(orderCountTask, productCountTask, totalInterest);

            //兴业站点
            Task<int> xyOrderCountTask = this.xyOrderInfoService.GetPaidCountAsync();
            Task<int> xyProductCountTask = this.xyProductInfoService.GetRepaidCountAsync();
            Task<decimal> xyTotalInterest = this.xyOrderInfoService.GetTotalInterestAsync();
            await Task.WhenAll(xyOrderCountTask, xyProductCountTask, xyTotalInterest);

            IndexStatistics indexStatistics = new IndexStatistics
            {
                OutletCount = 50,
                OrderCount = orderCountTask.Result + xyOrderCountTask.Result,
                ProductCount = productCountTask.Result + xyProductCountTask.Result,
                TotalInterest = totalInterest.Result + xyTotalInterest.Result
            };
            return this.Ok(indexStatistics);
        }
    }
}