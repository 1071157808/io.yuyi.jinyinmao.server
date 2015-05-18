// FileInformation: nyanya/Domain/CacheService.cs
// CreatedTime: 2014/06/03   4:54 PM
// LastUpdatedTime: 2014/06/05   4:19 PM

using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Infrastructure.Cache.Interface;
using Infrastructure.Lib.Extensions;

namespace Domain.ReadModel.Implementation
{
    public abstract class CacheService
    {
        protected readonly ICacheClient cacheClient;
        protected readonly ObjectCache objectCache;

        protected CacheService(ObjectCache objectCache, ICacheClient cacheClient)
        {
            this.objectCache = objectCache;
            this.cacheClient = cacheClient;
        }

        protected async Task RestoreToCache<T>(string key, T t, CacheItemPolicy policy = null, TimeSpan? timeSpan = null)
        {
            if (t.IsNull())
            {
                return;
            }

            this.RestoreToMemoryCache(key, t, policy);
            await this.cacheClient.Set(key, t, timeSpan);
        }

        protected void RestoreToMemoryCache<T>(string key, T t, CacheItemPolicy policy = null)
        {
            if (t.IsNull())
            {
                return;
            }

            if (policy == null)
            {
                policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.MaxValue };
            }

            this.objectCache.Set(key, t, policy);
        }
    }
}