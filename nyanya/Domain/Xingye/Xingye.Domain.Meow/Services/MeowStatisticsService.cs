// FileInformation: nyanya/Xingye.Domain.Meow/MeowStatisticsService.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/03   10:31 AM

using System.Threading.Tasks;
using Xingye.Domain.Meow.Services.DTO;
using Xingye.Domain.Meow.Services.Interfaces;
using Xingye.Domain.Orders.Services;
using Xingye.Domain.Orders.Services.Interfaces;
using Xingye.Domain.Products.Services;
using Xingye.Domain.Products.Services.Interfaces;

namespace Xingye.Domain.Meow.Services
{
    public class MeowStatisticsService : IMeowStatisticsService
    {
        private readonly IOrderInfoService orderInfoService;
        private readonly IProductInfoService productInfoService;

        public MeowStatisticsService()
        {
            this.orderInfoService = new OrderInfoService();
            this.productInfoService = new ProductInfoService();
        }

        public MeowStatisticsService(IOrderInfoService orderInfoService, IProductInfoService productInfoService)
        {
            this.orderInfoService = orderInfoService;
            this.productInfoService = productInfoService;
        }

        #region IMeowStatisticsService Members

        public async Task<IndexStatistics> GetIndexStatisticsAsync()
        {
            int productCount = await this.productInfoService.GetRepaidCountAsync();
            int orderCount = await this.orderInfoService.GetPaidCountAsync();
            //int outletCount;
            //using (MeowContext context = new MeowContext())
            //{
            //    string value = await context.ReadonlyQuery<MeowSetting>().Where(s => s.Key == "OutletCount").Select(s => s.Value).FirstOrDefaultAsync();
            //    value = value ?? "40";
            //    outletCount = Convert.ToInt32(value);
            //}

            return new IndexStatistics
            {
                OrderCount = orderCount,
                OutletCount = 50,
                ProductCount = productCount
            };
        }

        #endregion IMeowStatisticsService Members
    }
}