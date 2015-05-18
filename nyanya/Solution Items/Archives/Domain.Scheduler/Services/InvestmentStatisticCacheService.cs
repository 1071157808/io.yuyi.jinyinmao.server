// FileInformation: nyanya/Domain.Scheduler/InvestmentStatisticCacheService.cs
// CreatedTime: 2014/04/28   6:15 PM
// LastUpdatedTime: 2014/05/04   6:21 PM

using System;
using System.Web;
using System.Web.Caching;
using Domain.Scheduler.Models;
using Domain.Scheduler.ViewModels;
using Infrastructure.Cache.Couchbase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Domain.Scheduler.Services
{
    public class InvestmentStatisticCacheService : IInvestmentStatisticCacheService
    {
        #region IInvestmentStatisticCacheService Members

        public InvestmentStatistic GetCache(string key)
        {
            InvestmentStatistic cache = HttpRuntime.Cache.Get("InSt/" + key) as InvestmentStatistic;

            if (cache == null)
            {
                object o = InvestmentStatisticCache.Get(key);
                if (o == null)
                {
                    return null;
                }
                cache = JObject.Parse(o.ToString()).ToObject<InvestmentStatistic>();
                // 个人收益情况，一般只会连续观看2次，即打开App和查看分享页，而App首页使用的是手动刷新模式。同时该受益值同时受下单和使用道具影响，暂时业务需要更高的实时性
                HttpRuntime.Cache.Insert("InSt/" + key, cache, null, DateTime.Now.AddMinutes(3), Cache.NoSlidingExpiration);
            }

            return cache;
        }

        public InvestmentStatisticHistoryViewModel GetHistoryCache(string userGuid)
        {
            object o = InvestmentStatisticCache.GetHistory(userGuid);

            if (o == null)
            {
                return null;
            }

            return JObject.Parse(o.ToString()).ToObject<InvestmentStatisticHistoryViewModel>();
        }

        public InvestmentStatistic GetOverallCache()
        {
            string key = "Meow";
            InvestmentStatistic cache = HttpRuntime.Cache.Get("InSt/" + key) as InvestmentStatistic;

            if (cache == null)
            {
                object o = InvestmentStatisticCache.Get(key);
                if (o == null)
                {
                    return null;
                }
                cache = JObject.Parse(o.ToString()).ToObject<InvestmentStatistic>();
                // 整体收益情况，几乎每一个用户查看自己的收益情况，都会使用该值，同时该值对实时性要求低，所以使用绝对时间，10分钟更新一次即可
                HttpRuntime.Cache.Insert("InSt/" + key, cache, null, DateTime.Now.AddSeconds(600), Cache.NoSlidingExpiration);
            }

            return cache;
        }

        public bool SetHistoryCache(string userGuid, InvestmentStatisticHistoryViewModel newHistory)
        {
            string json = JsonConvert.SerializeObject(newHistory);
            return InvestmentStatisticCache.SetHistory(userGuid, json);
        }

        #endregion IInvestmentStatisticCacheService Members
    }
}