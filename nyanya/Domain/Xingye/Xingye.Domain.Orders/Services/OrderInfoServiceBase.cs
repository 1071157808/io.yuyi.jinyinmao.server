// FileInformation: nyanya/Xingye.Domain.Orders/OrderInfoServiceBase.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:26 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xingye.Domain.Orders.Database;
using Xingye.Domain.Orders.Models;
using Xingye.Domain.Orders.ReadModels;
using Xingye.Domain.Orders.Services.Interfaces;
using Domian.DTO;

namespace Xingye.Domain.Orders.Services
{
    public class OrderInfoServiceBase<TOrder> : IOrderInfoService<TOrder> where TOrder : OrderInfo
    {
        protected Func<OrderContext> OrderContextFactory
        {
            get { return () => new OrderContext(); }
        }

        #region IOrderInfoService<TOrder> Members

        public async Task<IPaginatedDto<TOrder>> GetFailedOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime)
        {
            return await this.GetOrderInfosAsync(o => o.UserIdentifier == userIdentifier && o.HasResult && !o.IsPaid, pageIndex, pageSize, strategy);
        }

        public async Task<TOrder> GetOrderInfoAsync(string userIdentifier, string orderIdentifier)
        {
            return await this.GetOrderInfoAsync(o => o.OrderIdentifier == orderIdentifier && o.UserIdentifier == userIdentifier);
        }

        public async Task<TOrder> GetOrderInfoAsync(string orderIdentifier)
        {
            return await this.GetOrderInfoAsync(o => o.OrderIdentifier == orderIdentifier);
        }

        public async Task<IPaginatedDto<TOrder>> GetOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime)
        {
            return await this.GetOrderInfosAsync(o => o.UserIdentifier == userIdentifier, pageIndex, pageSize, strategy);
        }

        public async Task<IPaginatedDto<TOrder>> GetPaidOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime)
        {
            return await this.GetOrderInfosAsync(o => o.UserIdentifier == userIdentifier && o.HasResult && o.IsPaid, pageIndex, pageSize, strategy);
        }

        public async Task<IPaginatedDto<TOrder>> GetPayingOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime)
        {
            return await this.GetOrderInfosAsync(o => o.UserIdentifier == userIdentifier && !o.HasResult, pageIndex, pageSize, strategy);
        }

        public async Task<IPaginatedDto<TOrder>> GetSuccessfulOrderInfosAsync(string userIdentifier, int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime)
        {
            return await this.GetOrderInfosAsync(o => o.UserIdentifier == userIdentifier && (o.IsPaid || !o.HasResult), pageIndex, pageSize, strategy);
        }

        #endregion IOrderInfoService<TOrder> Members

        public async Task<string> GetConsignmentAgreementAsync(string orderIdentifier)
        {
            return await this.GetAgreementInfoAsync(i => i.ConsignmentAgreementContent, orderIdentifier);
        }

        public async Task<string> GetPledgeAgreementAsync(string orderIdentifier)
        {
            return await this.GetAgreementInfoAsync(i => i.PledgeAgreementContent, orderIdentifier);
        }

        private async Task<string> GetAgreementInfoAsync(Expression<Func<AgreementsInfo, string>> selector, string orderIdentifier)
        {
            using (OrderContext context = new OrderContext())
            {
                return await context.ReadonlyQuery<AgreementsInfo>().Where(i => i.OrderIdentifier == orderIdentifier).Select(selector).FirstOrDefaultAsync();
            }
        }

        private async Task<TOrder> GetOrderInfoAsync(Expression<Func<TOrder, bool>> predicate)
        {
            using (OrderContext context = new OrderContext())
            {
                return await context.ReadonlyQuery<TOrder>().FirstOrDefaultAsync(predicate);
            }
        }

        private async Task<IPaginatedDto<TOrder>> GetOrderInfosAsync(Expression<Func<TOrder, bool>> predicate,
            int pageIndex = 1, int pageSize = 10, OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime)
        {
            int totalCount;
            IList<TOrder> items;

            using (OrderContext context = new OrderContext())
            {
                if (strategy == OrderSortingStrategy.BySettleDate)
                {
                    DateTime time = DateTime.Today.AddDays(1);
                    items = await context.ReadonlyQuery<TOrder>().Where(predicate).OrderBy(o => o.IsPaid)
                        .ThenByDescending(o => o.SettleDate < time).ThenBy(o => o.SettleDate).ThenByDescending(o => o.Id)
                        .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                }
                else
                {
                    items = await context.ReadonlyQuery<TOrder>().Where(predicate).OrderBy(o => o.IsPaid).ThenByDescending(o => o.OrderTime)
                        .ThenByDescending(o => o.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
                }

                totalCount = await context.ReadonlyQuery<TOrder>().Where(predicate).CountAsync();
            }

            return new PaginatedDto<TOrder>(pageIndex, pageSize, totalCount, items);
        }
    }
}