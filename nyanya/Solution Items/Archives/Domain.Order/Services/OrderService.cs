// FileInformation: nyanya/Domain.Order/OrderService.cs
// CreatedTime: 2014/05/13   8:14 AM
// LastUpdatedTime: 2014/05/14   2:25 PM

using System.Linq;
using System.Threading.Tasks;
using Domain.Order.Models;
using Domain.Order.Services.Interfaces;
using Infrastructure.Data.EntityFramework.Extensions;

namespace Domain.Order.Services
{
    public class OrderService : IOrderService
    {
        // 获取投资中的并且没有道具使用记录的订单，现在投资中的订单就是指已付款的订单或者起息中的订单

        #region IOrderService Members

        public async Task<PaginatedList<OrderListItem>> GetInvestingPaginatedOrdersAsync(int pageIndex, int pageSize, string userGuid)
        {
            using (OrderContext context = new OrderContext())
            {
                return await context.OrderListItems.AsNoTracking().OrderByDescending(i => i.Id).Where(i => i.UserGuid == userGuid && !i.ItemId.HasValue && i.Status == OrderStatus.Paid).ToPaginatedListAsync(pageIndex, pageSize);
            }
        }

        public async Task<PaginatedList<OrderListItem>> GetPaginatedOrdersAsync(int pageIndex, int pageSize, string userGuid)
        {
            using (OrderContext context = new OrderContext())
            {
                return await context.OrderListItems.AsNoTracking().OrderByDescending(i => i.Id).Where(i => i.UserGuid == userGuid).ToPaginatedListAsync(pageIndex, pageSize);
            }
        }

        #endregion IOrderService Members
    }
}