// FileInformation: nyanya/Xingye.Domain.Orders/OrderInfoService.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   3:30 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xingye.Commands.Orders;
using Xingye.Domain.Orders.Database;
using Xingye.Domain.Orders.Models;
using Xingye.Domain.Orders.ReadModels;
using Xingye.Domain.Orders.Services.DTO;
using Xingye.Domain.Orders.Services.Interfaces;

namespace Xingye.Domain.Orders.Services
{
    public class OrderInfoService : OrderInfoServiceBase<OrderInfo>, IOrderInfoService
    {
        #region IOrderInfoService Members

        public async Task<InvestingInfo> GetInvestingInfoAsync(string userIdentifier)
        {
            DateTime begin = DateTime.Today.Date;
            DateTime end = DateTime.Today.AddDays(1).Date;
            List<Order> orders;
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                orders = await context.ReadonlyQuery<Order>().Where(o => o.UserIdentifier == userIdentifier && o.PaymentInfo.IsPaid).ToListAsync();
            }

            decimal totalInterst = orders.Sum(o => o.Interest + o.ExtraInterest);
            IList<Order> investingOrders = orders.Where(o => o.SettleDate >= end && o.ValueDate <= begin).ToList();
            // 预期收益
            decimal interest = investingOrders.Sum(o => o.Interest + o.ExtraInterest);
            // 累积收益
            decimal repaidInterest = totalInterst - interest;
            // 在投资金
            decimal principal = investingOrders.Sum(o => o.Principal);

            return new InvestingInfo
            {
                Interest = interest,
                Principal = principal,
                TotalInterest = repaidInterest
            };
        }

        public async Task<decimal> GetMaxIncomeSpeedAsync(decimal yield = 7)
        {
            DateTime begin = DateTime.Today.Date;
            DateTime end = DateTime.Today.AddDays(1).Date;
            List<decimal> top;
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                top = await context.ReadonlyQuery<Order>().Where(o => o.PaymentInfo.IsPaid && o.SettleDate >= end && o.ValueDate <= begin)
                    .OrderByDescending(o => o.Principal).Select(o => o.Principal).Take(1).ToListAsync();
            }
            return top.Count > 0 ? top.First() * yield / 100 : yield;
        }

        public async Task<int> GetPaidCountAsync() 
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.Database.SqlQuery<int>("Select count(1) from PaymentInfo where IsPaid=1").FirstAsync();
            }
        }

        public async Task<IList<SettlingProductInfo>> GetSettlingProductInfosAsync(string userIdentifier)
        {
            List<SettlingOrder> settlingOrders;
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                settlingOrders = await context.ReadonlyQuery<SettlingOrder>()
                    .Where(o => o.UserIdentifier == userIdentifier && o.SettleDate >= DateTime.Today.Date)
                    .OrderBy(o => o.SettleDate)
                    .Take(3).ToListAsync();
            }

            return settlingOrders.Select(o => new SettlingProductInfo
            {
                ExtraInterest = o.ExtraInterest,
                Interest = o.Interest, // + o.ExtraInterest,//去除额外收益
                Principal = o.Principal,
                ProductIdentifier = o.ProductIdentifier,
                ProductName = o.ProductName,
                ProductNo = o.ProductNo,
                ProductNumber = o.ProductNumber,
                ProductType = o.OrderType.ToProductType(),
                SettleDate = o.SettleDate
            }).ToList();
        }

        public async Task<decimal> GetTheUserIncomeSpeedAsync(string userIdentifier)
        {
            DateTime begin = DateTime.Today.Date;
            DateTime end = DateTime.Today.AddDays(1).Date;
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                var orders = await context.ReadonlyQuery<Order>()
                    .Where(o => o.UserIdentifier == userIdentifier && o.PaymentInfo.IsPaid && o.SettleDate >= end && o.ValueDate <= begin)
                    .Select(o => new { o.Principal, o.Yield }).ToListAsync();
                return orders.Count > 0 ? orders.Sum(o => o.Principal * o.Yield) / 100 : 0;
            }
        }

        /// <summary>
        /// 获取累计总收益
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetTotalInterestAsync()
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.Database.SqlQuery<decimal>("SELECT case when SUM(o.Interest) is null then 0 else SUM(o.Interest) end as Interest FROM Orders AS o INNER JOIN PaymentInfo AS p ON p.OrderIdentifier=o.OrderIdentifier WHERE p.IsPaid=1").FirstAsync();
            }
        }

        #endregion IOrderInfoService Members
    }
}