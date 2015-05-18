// FileInformation: nyanya/Domain.Meow/IItemService.cs
// CreatedTime: 2014/05/08   9:18 AM
// LastUpdatedTime: 2014/05/08   3:00 PM

using System;
using System.Threading.Tasks;
using Domain.Meow.Models;
using Infrastructure.Data.EntityFramework.Extensions;

namespace Domain.Meow.Services.Interfaces
{
    public interface IItemService
    {
        bool CheckNewArrival(string userGuid);

        Task CollectActivitisItems(string userGuid, DateTime expires);

        Task DistributeOHPItemAsync(string userGuid, DateTime expires);

        PaginatedList<Item> GetPaginatedItems(int pageIndex, int pageSize, string userGuid);

        Task<PaginatedList<Item>> GetPaginatedItemsAsync(int pageIndex, int pageSize, string userGuid);

        Task<PaginatedList<Item>> GetPaginatedUseableItemsAsync(int pageIndex, int pageSize, string userGuid);

        Task RemoveItem(int itemId, string userGuid);

        void RemoveNewItemFlag(string userGuid);

        void SetNewItemFlag(string userGuid);
    }
}