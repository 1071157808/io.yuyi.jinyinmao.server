// FileInformation: nyanya/Domain.Order/OrderSummaryService.cs
// CreatedTime: 2014/04/01   3:04 PM
// LastUpdatedTime: 2014/04/29   5:09 PM

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Order.Models;
using Domain.Order.Services.Interfaces;

namespace Domain.Order.Services
{
    public class OrderSummaryService : IOrderSummaryService
    {
        #region IOrderSummaryService Members

        public async Task<List<OrderSummary>> GetIntereSettledOrders(string userGuid)
        {
            using (OrderContext db = new OrderContext())
            {
                return await db.OrderSummaries.Where(o => o.UserGuid == userGuid && o.OrderStatus == OrderStatus.InterestSettled).ToListAsync();
            }
        }

        public async Task<List<OrderSummary>> GetInvestingOrders(string userGuid)
        {
            using (OrderContext db = new OrderContext())
            {
                return await db.OrderSummaries.Where(o => o.UserGuid == userGuid &&
                                                          (o.OrderStatus == OrderStatus.Paid || o.OrderStatus == OrderStatus.InterestAccruing)).ToListAsync();
            }
        }

        public async Task<InvestmentSummaryDto> GetInvestingSummary(string userGuid)
        {
            List<OrderSummary> investingOrders = await this.GetInvestingOrders(userGuid);

            decimal investingPrice = investingOrders.Sum(o => o.Price.GetValueOrDefault());
            decimal expectedEarnings = investingOrders.Sum(o => o.ExpectedPrice.GetValueOrDefault());

            return new InvestmentSummaryDto { Earnings = 0, ExpectedEarnings = expectedEarnings, InvestingPrice = investingPrice };
        }

        public async Task<InvestmentSummaryDto> GetInvestmentSummary(string userGuid)
        {
            List<OrderSummary> intereSettledOrders = await this.GetIntereSettledOrders(userGuid);
            List<OrderSummary> investingOrders = await this.GetInvestingOrders(userGuid);

            decimal earnings = intereSettledOrders.Sum(o => o.ExpectedPrice.GetValueOrDefault());

            decimal investingPrice = investingOrders.Sum(o => o.Price.GetValueOrDefault());
            decimal expectedEarnings = investingOrders.Sum(o => o.ExpectedPrice.GetValueOrDefault());

            return new InvestmentSummaryDto { Earnings = earnings, ExpectedEarnings = expectedEarnings, InvestingPrice = investingPrice };
        }

        #endregion IOrderSummaryService Members
    }
}