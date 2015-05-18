using Domian.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xingye.Domain.Orders.Database;
using Xingye.Domain.Orders.ReadModels;
using Xingye.Domain.Orders.Services.Interfaces;

namespace Xingye.Domain.Orders.Services
{
    public class XYOrderInfoService : OrderInfoServiceBase<OrderInfo>, IXYOrderInfoService
    {
        public async Task<IPaginatedDto<OrderInfo>> GetOrderInfosBySqlQueryAsync(string userIdentifier, DateTime startTime, DateTime endTime, int pageIndex = 1,
            int orderMode = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime)
        {
            int totalCount = 0;
            IList<OrderInfo> items = new OrderInfo[1];
            string tableName = "dbo.AllBAOrderInfo";
            string subQuery = "(select * ,row_number() over(order by  ispaid,OrderTime desc) rn from dbo.OrderInfo ";
            string whereQuery = "WHERE InvestorUserIdentifier='" + userIdentifier + "'  ";
            switch (orderMode)
            {
                case 2:
                    whereQuery += "and (IsPaid=1 or HasResult=0) ";
                    break;

                case 3:
                    whereQuery += "and (IsPaid=0 and HasResult=1) ";
                    break;
            }
            if (!startTime.Equals(DateTime.MinValue))
            {
                whereQuery += "and  OrderTime>'" + startTime.ToString("yyyy-MM-dd") + "'";
            }
            if (!endTime.Equals(DateTime.MaxValue))
            {
                whereQuery += "and OrderTime<='" + endTime.ToString("yyyy-MM-dd") + "'";
            }
            int firstItemNo = (pageIndex - 1) * pageSize;
            int lastItemNo = firstItemNo + pageSize;
            string pageInfo = " where rn > " + firstItemNo + "  and rn<=" + lastItemNo;
            subQuery += whereQuery + " ) a" + pageInfo;
            using (OrderContext context = new OrderContext())
            {
                try
                {
                    DateTime time = DateTime.Today.AddDays(1);
                    items = await context.Database.SqlQuery<OrderInfo>("select * from " + subQuery).ToArrayAsync();
                    //"(select * ,row_number() over(order by  ispaid,OrderTime desc) rn from dbo.AllBAOrderInfo WHERE InvestorUserIdentifier='92575efaff0d443d9a297f1fe714558e' and (IsPaid=1 or HasResult=0) and OrderTime<='2015-1-1' and  OrderTime>'2013-1-1' ) a where rn >= 1  and rn<51");
                    //                    items = await context.ReadonlyQuery<BAOrderInfo>().Where(predicate).OrderBy(o => o.IsPaid)
                    //                        .ThenByDescending(o => o.SettleDate < time).ThenBy(o => o.SettleDate).ThenByDescending(o => o.Id)
                    //                        .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();

                    totalCount = await context.Database.ExecuteSqlCommandAsync("select count(*) from " + subQuery);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }
            }

            return new PaginatedDto<OrderInfo>(pageIndex, pageSize, totalCount, items);
        }

        public async Task<IPaginatedDto<OrderInfo>> GetOrderInfosAsync(string userIdentifier, DateTime startTime, DateTime endTime, int pageIndex = 1,
            OrderMode orderMode = OrderMode.AllOrder, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime)
        {
            int totalCount;
            IList<OrderInfo> items;

            using (OrderContext context = new OrderContext())
            {
                IQueryable<OrderInfo> queryable = context.ReadonlyQuery<OrderInfo>().Where(o => o.UserIdentifier == userIdentifier);
                queryable = CreateOrderTimeQuery(startTime, endTime, orderMode, queryable);
                var orderedQueryable = CreateOrderedQueryable(strategy, queryable);
                items = await orderedQueryable.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                totalCount = await queryable.CountAsync();
            }

            return new PaginatedDto<OrderInfo>(pageIndex, pageSize, totalCount, items);
        }

        private static IOrderedQueryable<OrderInfo> CreateOrderedQueryable(OrderSortingStrategy strategy, IQueryable<OrderInfo> queryable)
        {
            IOrderedQueryable<OrderInfo> orderedQueryable = queryable.OrderByDescending(o => o.OrderTime)
                .ThenByDescending(o => o.Id);
            DateTime time = DateTime.Today.AddDays(1);
            switch (strategy)
            {
                case OrderSortingStrategy.BySettleDate:
                    orderedQueryable =
                        queryable.OrderBy(o => o.IsPaid)
                            .ThenByDescending(o => o.SettleDate < time)
                            .ThenBy(o => o.SettleDate)
                            .ThenByDescending(o => o.Id);
                    break;
            }
            return orderedQueryable;
        }

        private static IQueryable<OrderInfo> CreateOrderTimeQuery(DateTime startTime, DateTime endTime, OrderMode orderMode, IQueryable<OrderInfo> queryable)
        {
            queryable = CreateOrderModeQuery(orderMode, queryable);
            if (!startTime.Equals(DateTime.MinValue))
            {
                queryable = queryable.Where(o => DbFunctions.TruncateTime(o.OrderTime) >= startTime);
            }
            if (!endTime.Equals(DateTime.MaxValue))
            {
                queryable = queryable.Where(o => DbFunctions.TruncateTime(o.OrderTime) <= endTime);
            }
            return queryable;
        }

        private static IQueryable<OrderInfo> CreateOrderModeQuery(OrderMode orderMode, IQueryable<OrderInfo> queryable)
        {
            switch (orderMode)
            {
                case OrderMode.SuccessfulOrder:
                    queryable = queryable.Where(o => o.IsPaid || !o.HasResult);
                    break;

                case OrderMode.FailedOrder:
                    queryable = queryable.Where(o => !o.IsPaid && o.HasResult);
                    break;
            }
            return queryable;
        }
    }
}