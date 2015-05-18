using Domian.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xingye.Domain.Orders.ReadModels;

namespace Xingye.Domain.Orders.Services.Interfaces
{
    public interface IXYOrderInfoService : IOrderInfoService<OrderInfo>
    {
        Task<IPaginatedDto<OrderInfo>> GetOrderInfosAsync(string userIdentifier, DateTime startTime, DateTime endTime,
           int pageIndex = 1, OrderMode orderMode = OrderMode.AllOrder, int pageSize = 10,
           OrderSortingStrategy strategy = OrderSortingStrategy.ByOrderTime);
    }
}