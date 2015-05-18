// FileInformation: nyanya/Infrastructure.Cache.Couchbase/InvestmentStatisticCache.cs
// CreatedTime: 2014/04/25   4:28 PM
// LastUpdatedTime: 2014/04/29   2:34 PM

using System.Configuration;
using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching.Memcached;

namespace Infrastructure.Cache.Couchbase
{
    public class InvestmentStatisticCache
    {
        private static readonly CouchbaseClient couchbaseClient;

        static InvestmentStatisticCache()
        {
            CouchbaseClientSection couchbaseClientSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/InvestmentStatistic");
            couchbaseClient = new CouchbaseClient(couchbaseClientSection);
        }

        public static object Get(string key)
        {
            return couchbaseClient.Get(key);
        }

        public static object GetHistory(string key)
        {
            return couchbaseClient.Get("h" + key);
        }

        public static bool Set(string key, string json)
        {
            bool result;
            int times = 0;

            do
            {
                times++;
                result = couchbaseClient.Store(StoreMode.Set, key, json);
            } while (!result && times <= 10);

            return result;
        }

        public static bool SetHistory(string key, string json)
        {
            bool result;
            int times = 0;

            do
            {
                times++;
                result = couchbaseClient.Store(StoreMode.Set, "h" + key, json);
            } while (!result && times <= 10);

            return result;
        }
    }
}