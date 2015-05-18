using System;
using System.Threading.Tasks;
using System.Runtime.Caching;
using Cat.Domain.Orders.ReadModels;
using Cat.Domain.Orders.Services.Interfaces;
using Infrastructure.Lib.Extensions;
using Newtonsoft.Json;
using nyanya.Meow.Services.Interfaces;
using Cat.Domain.Orders.Services;

namespace nyanya.Meow.Services
{
    /// <summary>
    ///   TimelineService
    /// </summary>
    public class TimelineService : IRuntimeCachedTimelineService
    {
        private const string KeyFormat = "TL_{0}";
        private readonly ITimelineInfoService timelineInfoService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineService"/> class.
        /// </summary>
        public TimelineService()
        {
            this.timelineInfoService = new TimelineService();
        }

        private MemoryCache Cache
        {
            get { return MemoryCache.Default; }
        }

        /// <summary>
        /// Gets the timeline asynchronous.
        /// </summary>
        /// <param name="userIdentifier">The user identifier.</param>
        /// <returns></returns>
        public async Task<Timeline> GetTimelineAsync(string userIdentifier)
        {
            Timeline timeline;
            object data = Cache.Get(KeyFormat.FormatWith(userIdentifier));
            if (data != null)
            {
                try
                {
                    timeline = JsonConvert.DeserializeObject<Timeline>(data.ToString());
                    return timeline;
                }
                catch
                {
                    //ignore
                }
            }

            timeline = await timelineInfoService.GetTimelineAsync(userIdentifier);

            // ReSharper disable once CSharpWarnings::CS4014
            Task.Run(() => this.RestoreDataToCache(KeyFormat.FormatWith(userIdentifier), timeline));

            return timeline;
        }

        private void RestoreDataToCache(string key, Timeline timeline)
        {
            Cache.Set(new CacheItem(key, JsonConvert.SerializeObject(timeline)), new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromMinutes(3),
            });
        }
    }
}