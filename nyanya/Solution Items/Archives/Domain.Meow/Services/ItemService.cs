// FileInformation: nyanya/Domain.Meow/ItemService.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/06/10   4:16 PM

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Meow.Models;
using Domain.Meow.Services.Interfaces;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Data.EntityFramework.Extensions;

namespace Domain.Meow.Services
{
    public class ItemService : IItemService
    {
        #region IItemService Members

        public bool CheckNewArrival(string userGuid)
        {
            return ItemNewArrivalCache.CheckNewItemFlag(userGuid);
        }

        public async Task CollectActivitisItems(string userGuid, DateTime expires)
        {
            OHPItem ohpItem = ItemFactory<OHPItem>.CreateItem();
            ohpItem.OwnerGuid = userGuid;
            ohpItem.Expires = expires;
            using (MeowContext meowContext = new MeowContext())
            {
                if (meowContext.OHPItems.Count(i => i.OwnerGuid == userGuid) == 0)
                {
                    meowContext.OHPItems.Add(ohpItem);
                    await meowContext.SaveChangesAsync();
                    this.SetNewItemFlag(userGuid);
                }
            }
        }

        public async Task DistributeOHPItemAsync(string userGuid, DateTime expires)
        {
            OHPItem ohpItem = ItemFactory<OHPItem>.CreateItem();
            ohpItem.OwnerGuid = userGuid;
            ohpItem.Expires = expires;
            using (MeowContext meowContext = new MeowContext())
            {
                meowContext.OHPItems.Add(ohpItem);
                await meowContext.SaveChangesAsync();
            }
        }

        public PaginatedList<Item> GetPaginatedItems(int pageIndex, int pageSize, string userGuid)
        {
            using (MeowContext context = new MeowContext())
            {
                return context.Items.AsNoTracking().Include(i => i.Category).OrderByDescending(i => i.Id).Where(i => i.OwnerGuid == userGuid && !i.IsUsed).ToPaginatedList(pageIndex, pageSize);
            }
        }

        public async Task<PaginatedList<Item>> GetPaginatedItemsAsync(int pageIndex, int pageSize, string userGuid)
        {
            using (MeowContext context = new MeowContext())
            {
                return await context.Items.AsNoTracking().Include(i => i.Category).OrderByDescending(i => i.Id).Where(i => i.OwnerGuid == userGuid && !i.IsUsed).ToPaginatedListAsync(pageIndex, pageSize);
            }
        }

        public async Task<PaginatedList<Item>> GetPaginatedUseableItemsAsync(int pageIndex, int pageSize, string userGuid)
        {
            //2014-04-22 18：03
            //!HACK:没有任何逻辑判断，只返回所有道具，只适合暂时的业务逻辑
            using (MeowContext context = new MeowContext())
            {
                return await context.Items.AsNoTracking().Include(i => i.Category).OrderByDescending(i => i.Id).Where(i => i.OwnerGuid == userGuid && !i.IsUsed && DateTime.Now < i.Expires).ToPaginatedListAsync(pageIndex, pageSize);
            }
        }

        public async Task RemoveItem(int itemId, string userGuid)
        {
            using (MeowContext context = new MeowContext())
            {
                Item item = await context.Items.Where(i => i.Id == itemId && i.OwnerGuid == userGuid && i.Expires <= DateTime.Now && !i.IsUsed).FirstOrDefaultAsync();

                if (item != null)
                {
                    context.Items.Remove(item);
                    await context.SaveChangesAsync();
                }
            }
        }

        public void RemoveNewItemFlag(string userGuid)
        {
            Task.Run(() => ItemNewArrivalCache.RemoveNewItemFlag(userGuid));
        }

        public void SetNewItemFlag(string userGuid)
        {
            Task.Run(() => ItemNewArrivalCache.SetNewItemFlag(userGuid));
        }

        #endregion IItemService Members
    }
}