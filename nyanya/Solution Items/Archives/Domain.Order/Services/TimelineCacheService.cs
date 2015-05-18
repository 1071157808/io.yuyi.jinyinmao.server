// FileInformation: nyanya/Domain.Order/TimelineCacheService.cs
// CreatedTime: 2014/04/29   6:19 PM
// LastUpdatedTime: 2014/05/09   4:33 PM

using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Domain.Order.Models;
using Infrastructure.Cache.Couchbase;
using Newtonsoft.Json.Linq;

namespace Domain.Order.Services.Interfaces
{
    public class TimelineCacheService : ITimelineCacheService
    {
        #region ITimelineCacheService Members

        public async Task<Timeline> GetCache(string key, bool refresh)
        {
            Timeline cache = HttpRuntime.Cache.Get("TL/" + key) as Timeline;

            if (cache == null)
            {
                cache = await GetDistributedCache(key);
            }
            else
            {
                if (refresh)
                {
                    // ReSharper disable once CSharpWarnings::CS4014
                    GetDistributedCache(key);
                }
            }

            return cache;
        }

        private static Task<Timeline> GetDistributedCache(string key)
        {
            return Task.Run(() =>
            {
                object o = TimelineCache.Get(key);

                if (o == null)
                {
                    return null;
                }
                Timeline distributedCache = JObject.Parse(o.ToString()).ToObject<Timeline>();
                // 时间线使用SlidingExpiration模式，主要因为时间线暂时只受下单的影响，同时时间线对实时性要求较低，而且需要维持用户在一次时间线使用上体验的连续性
                // 2014-05-04 14:06 调整为3分钟
                // 2014-05-09 14:25 调整为1分30秒
                HttpRuntime.Cache.Insert("TL/" + key, distributedCache, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 1, 30));
                return distributedCache;
            });
        }

        #endregion ITimelineCacheService Members
    }
}