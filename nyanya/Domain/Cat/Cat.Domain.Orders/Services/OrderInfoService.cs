// FileInformation: nyanya/Cat.Domain.Orders/OrderInfoService.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/17   9:31 AM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cat.Commands.Orders;
using Cat.Domain.Orders.Database;
using Cat.Domain.Orders.Models;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.DTO;
using Cat.Domain.Orders.Services.Interfaces;
using Cat.Domain.Users.Services;
using System.Data.SqlClient;
using Infrastructure.Lib.Extensions;

namespace Cat.Domain.Orders.Services
{
    public class OrderInfoService : OrderInfoServiceBase<OrderInfo>, IExactOrderInfoService
    {
        #region IExactOrderInfoService Members

        public async Task<InvestingInfo> GetInvestingInfoAsync(string userIdentifier)
        {
            DateTime begin = DateTime.Today.Date;
            DateTime end = DateTime.Today.AddDays(1).Date;
            List<Order> orders;
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                orders = await context.ReadonlyQuery<Order>().Where(o => o.UserIdentifier == userIdentifier && o.PaymentInfo.IsPaid && o.OrderType != OrderType.ZCBAcceptance).ToListAsync();
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
                top = await context.ReadonlyQuery<Order>().Where(o => o.PaymentInfo.IsPaid && o.SettleDate >= end && o.ValueDate <= begin && o.OrderType != OrderType.ZCBAcceptance)
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
                    .Where(o => o.UserIdentifier == userIdentifier)
                    .OrderBy(o => o.SettleDate)
                    .Take(3).ToListAsync();
            }

            return settlingOrders.Select(o => new SettlingProductInfo
            {
                ExtraInterest = o.ExtraInterest,
                Interest = o.Interest, // + o.ExtraInterest,    //去除额外收益
                Principal = o.Principal,
                ProductIdentifier = o.ProductIdentifier,
                ProductName = o.ProductName,
                ProductNo = o.ProductNo,
                ProductNumber = o.ProductNumber,
                ProductType = o.OrderType.ToProductType(),
                SettleDate = o.SettleDate,
                Category = o.ProductCategory
            }).ToList();
        }

        public async Task<IList<string>> GetUnResultOrdersAsync(DateTime time)
        {
            IList<OrderInfo> unResultOrders;

            
            using (OrderContext context = new OrderContext())
            {
                DateTime starTime = time.AddHours(-72);
                unResultOrders = await context.ReadonlyQuery<OrderInfo>().Where(p => p.HasResult == false && p.OrderTime < time && starTime < p.OrderTime).ToArrayAsync();
            }
            return unResultOrders.Select(p => p.OrderIdentifier).ToArray();
        }

        public async Task<decimal> GetTheUserIncomeSpeedAsync(string userIdentifier)
        {
            DateTime begin = DateTime.Today.Date;
            DateTime end = DateTime.Today.AddDays(1).Date;
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                var orders = await context.ReadonlyQuery<Order>()
                    .Where(o => o.UserIdentifier == userIdentifier && o.PaymentInfo.IsPaid && o.SettleDate >= end && o.ValueDate <= begin && o.OrderType != OrderType.ZCBAcceptance)
                    .Select(o => new { o.Principal, o.Yield }).ToListAsync();
                return orders.Count > 0 ? orders.Sum(o => o.Principal * o.Yield) / 100 : 0;
            }
        }

        public async Task<IList<OrderInfo>> GetTheUserOrders(string userIdentifier, string productIdentifier)
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<OrderInfo>()
                    .Where(o => o.UserIdentifier == userIdentifier && o.IsPaid && o.ProductIdentifier == productIdentifier).ToListAsync();
            }
        }

        public async Task<List<OrderInfo>> GetTheUserActivityOrders(string userIdentifier)
        {
            using (OrderContext context=this.OrderContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<OrderInfo>()
                    .Where(
                        o =>
                            o.UserIdentifier == userIdentifier 
                            && ((o.IsPaid && o.HasResult) || (!o.HasResult && !o.IsPaid)) 
                            && o.ExtraInterest > 0)
                    .ToListAsync();
            }
        }

        public async Task<IList<OrderInfo>> GetTheUserFixDateActivityOrders(string userIdentifier, DateTime nowDateTime)
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<OrderInfo>()
                    .Where(
                        o =>
                            o.UserIdentifier == userIdentifier
                            && ((o.IsPaid && o.HasResult) || (!o.HasResult && !o.IsPaid))
                            && (DbFunctions.TruncateTime(o.OrderTime) == nowDateTime)
                            && o.ExtraInterest > 0)
                    .ToListAsync();
            }
        }

        public async Task<IList<OrderInfo>> GetTheUserBeforeDateActivityOrders(string userIdentifier, DateTime beforeDateTime)
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<OrderInfo>()
                    .Where(
                        o =>
                            o.UserIdentifier == userIdentifier
                            && (o.IsPaid && o.HasResult) 
                            && o.OrderTime < beforeDateTime
                            && o.ExtraInterest > 0)
                    .ToListAsync();
            }
        }

        public async Task<IList<OrderInfo>> GetTheUserBetweenDateActivityOrders(string userIdentifier, DateTime beginTime, DateTime endTime)
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<OrderInfo>()
                    .Where(
                        o =>
                            o.UserIdentifier == userIdentifier
                            && ((o.IsPaid && o.HasResult) || (!o.IsPaid && !o.HasResult))
                            && o.OrderTime >= beginTime
                            && o.OrderTime < endTime
                            && o.ExtraInterest > 0)
                    .ToListAsync();
            }
        }

        public async Task<List<OrderInfo>> GetTheUserOrdersForLuckhub(string userIdentifier, DateTime beginTime, DateTime endTime)
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<OrderInfo>().Where(o => 
                                                                          o.UserIdentifier == userIdentifier 
                                                                          && o.OrderTime >= beginTime
                                                                          && o.OrderTime < endTime)
                                                               .ToListAsync();
            }
        }

        /// <summary>
        /// 累计总收益
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetTotalInterestAsync()
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.Database.SqlQuery<decimal>("SELECT case when SUM(o.Interest) is null then 0 else SUM(o.Interest) end as Interest FROM Orders AS o INNER JOIN PaymentInfo AS p ON p.OrderIdentifier=o.OrderIdentifier WHERE p.IsPaid=1").FirstAsync();
            }
        }

        public async Task<decimal> GetTotalSaleAmountByProductAsync(string productIdentifier)
        {
            var sqlcmd = "select IsNull(Sum(TotalPrincipal),0) from ZCBUser where ProductIdentifier = '{0}'".FmtWith(productIdentifier);
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.Database.SqlQuery<decimal>(sqlcmd).FirstAsync();
            }
        }

        public async Task<decimal> GetTotalInterestByProductAsync(string productIdentifier)
        {
            var sqlcmd = "select IsNull(Sum(TotalInterest),0) from ZCBUser where ProductIdentifier = '{0}'".FmtWith(productIdentifier);
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.Database.SqlQuery<decimal>(sqlcmd).FirstAsync();
            }
        }

        public async Task<int> GetTotalCurrentAmountByProductAsync(string productIdentifier)
        {
            var sqlcmd = "select IsNull(Sum(CurrentPrincipal),0) from ZCBUser where ProductIdentifier = '{0}'".FmtWith(productIdentifier);
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return (int)await context.Database.SqlQuery<decimal>(sqlcmd).FirstAsync();
            }
        }

        public async Task<int> GetTotalRedeemInterestByProductAsync(string productIdentifier)
        {
            var sqlcmd = "select IsNull(Sum(TotalRedeemInterest),0) from ZCBUser where ProductIdentifier = '{0}'".FmtWith(productIdentifier);
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return (int)await context.Database.SqlQuery<decimal>(sqlcmd).FirstAsync();
            }
        }

        public async Task<IList<string>> GetZcbOrderIdentifierList()
        {
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                return await context.ReadonlyQuery<ZCBOrder>().Include(p => p.PaymentInfo)
                    .Where(x => x.PaymentInfo.IsPaid)
                    .Select(x => x.OrderIdentifier)
                    .ToListAsync();
            }
        }

        #endregion IExactOrderInfoService Members

        public async Task<Timeline> GetTimelineAsync(string userIdentifier)
        {
            List<TimelineOrder> timelineOrders;
            using (OrderContext context = this.OrderContextFactory.Invoke())
            {
                timelineOrders = await context.ReadonlyQuery<TimelineOrder>()
                    .Where(o => o.UserIdentifier == userIdentifier).ToListAsync();
            }

            IEnumerable<TimelineNode> orderNodes = timelineOrders.Select(o => new TimelineNode
            {
                Identifier = o.OrderIdentifier,
                Interest = o.Interest,
                Principal = o.Principal,
                Time = o.OrderTime,
                Type = TimelineNodeType.Order
            });

            IEnumerable<TimelineNode> productNodes = timelineOrders.GroupBy(o => o.ProductIdentifier)
                .Select(group => new TimelineNode
                {
                    Identifier = group.Key,
                    Interest = group.Sum(o => o.Interest),
                    Principal = group.Sum(o => o.Principal),
                    Time = group.First().RepaymentDeadline,
                    Type = TimelineNodeType.Product
                });

            DateTime signUpTime = (await (new UserInfoService()).GetUserInfoAsync(userIdentifier)).SignUpTime;
            TimelineNode beginningNode = new TimelineNode
            {
                Identifier = userIdentifier,
                Interest = 0,
                Principal = 0,
                Time = signUpTime,
                Type = TimelineNodeType.Beginning
            };

            TimelineNode todayNode = new TimelineNode
            {
                Identifier = userIdentifier,
                Interest = timelineOrders.Sum(o => o.Interest),
                Principal = timelineOrders.Sum(o => o.Principal),
                Time = DateTime.Today,
                Type = TimelineNodeType.Today
            };

            TimelineNode endNode = new TimelineNode
            {
                Identifier = userIdentifier,
                Interest = timelineOrders.Sum(o => o.Interest),
                Principal = timelineOrders.Sum(o => o.Principal),
                Time = DateTime.MaxValue,
                Type = TimelineNodeType.End
            };

            Timeline timeline = new Timeline();
            timeline.Nodes.Add(beginningNode);
            timeline.Nodes.AddRange(orderNodes);
            timeline.Nodes.Add(todayNode);
            timeline.Nodes.AddRange(productNodes);
            timeline.Nodes.Add(endNode);
            timeline.Nodes = timeline.Nodes.OrderBy(n => n.Time).ToList();

            return timeline;
        }
    }
}
