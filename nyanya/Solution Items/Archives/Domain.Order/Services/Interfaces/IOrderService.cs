// FileInformation: nyanya/Domain.Order/IOrderService.cs
// CreatedTime: 2014/04/23   12:35 AM
// LastUpdatedTime: 2014/04/23   12:41 AM

using System.Threading.Tasks;
using Domain.Order.Models;
using Infrastructure.Data.EntityFramework.Extensions;

namespace Domain.Order.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PaginatedList<OrderListItem>> GetInvestingPaginatedOrdersAsync(int pageIndex, int pageSize, string userGuid);

        Task<PaginatedList<OrderListItem>> GetPaginatedOrdersAsync(int pageIndex, int pageSize, string userGuid);
    }
}