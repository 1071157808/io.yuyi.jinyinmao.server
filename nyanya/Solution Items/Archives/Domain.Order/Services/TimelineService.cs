// FileInformation: nyanya/Domain.Order/TimelineService.cs
// CreatedTime: 2014/04/26   6:12 PM
// LastUpdatedTime: 2014/04/27   2:44 PM

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Order.Models;
using Domain.Order.Services;
using Domain.Order.Services.Interfaces;

namespace Domain.Meow.Services
{
    public class TimelineService : ITimelineService
    {
        #region ITimelineService Members

        public async Task<Timeline> GetFutureItemsAsync(string userGuid, int skip, int take)
        {
            return await this.GetFutureItemsFromCache(userGuid, skip, take) ?? await this.GetFutureItemsFromDatabaseAsync(userGuid, skip, take);
        }

        public async Task<Timeline> GetPastItemsAsync(string userGuid, int skip, int take)
        {
            return await this.GetPastItemsFromCache(userGuid, skip, take) ?? await this.GetPastItemsFromDatabaseAsync(userGuid, skip, take);
        }

        public async Task<string> GetTimelineTimestamp(string userGuid)
        {
            Timeline timeline = await this.GetItemsFromCache(userGuid, false);
            return timeline == null ? DateTime.Today.ToString("yyyyMMddHHmm_ss").Remove(14) : timeline.Timestamp;
        }

        #endregion ITimelineService Members

        private async Task<Timeline> GetFutureItemsFromCache(string userGuid, int skip, int take)
        {
            Timeline timeline = await this.GetItemsFromCache(userGuid, skip != 0);
            if (timeline == null)
            {
                return null;
            }

            return new Timeline { Items = timeline.Items.Where(i => i.Time >= DateTime.Today).Skip(skip).Take(take).ToList(), Timestamp = timeline.Timestamp };
        }

        private async Task<Timeline> GetFutureItemsFromDatabaseAsync(string userGuid, int skip, int take)
        {
            Timeline timeline = await this.GetItemsFromDatabaseAsync(userGuid);

            return new Timeline { Items = timeline.Items.Where(i => i.Time >= DateTime.Today).Skip(skip).Take(take).ToList(), Timestamp = timeline.Timestamp };
        }

        private async Task<Timeline> GetItemsFromCache(string userGuid, bool refreshCache)
        {
            ITimelineCacheService cacheService = new TimelineCacheService();
            return await cacheService.GetCache(userGuid, refreshCache);
        }

        private async Task<Timeline> GetItemsFromDatabaseAsync(string userGuid)
        {
            string newTimestamp = DateTime.Today.ToString("yyyyMMddHHmm_ss").Remove(14);
            List<TimelineItem> timelineItems;
            using (OrderContext orderContext = new OrderContext())
            {
                timelineItems = await orderContext.TimelineItems.AsNoTracking().OrderBy(i => i.Time).ThenBy(i => i.Type).ThenBy(i => i.Identifier)
                    .Where(i => i.UserGuid == userGuid).ToListAsync();
            }

            return new Timeline { Items = timelineItems, Timestamp = newTimestamp };
        }

        private async Task<Timeline> GetPastItemsFromCache(string userGuid, int skip, int take)
        {
            Timeline timeline = await this.GetItemsFromCache(userGuid, skip != 0);
            if (timeline == null)
            {
                return null;
            }

            return new Timeline { Items = timeline.Items.Where(i => i.Time < DateTime.Today).OrderByDescending(i => i.Time).Skip(skip).Take(take).ToList(), Timestamp = timeline.Timestamp };
        }

        private async Task<Timeline> GetPastItemsFromDatabaseAsync(string userGuid, int skip, int take)
        {
            Timeline timeline = await this.GetItemsFromDatabaseAsync(userGuid);

            return new Timeline { Items = timeline.Items.Where(i => i.Time < DateTime.Today).OrderByDescending(i => i.Time).Skip(skip).Take(take).ToList(), Timestamp = timeline.Timestamp };
        }
    }
}