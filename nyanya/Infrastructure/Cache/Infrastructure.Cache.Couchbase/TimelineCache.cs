// FileInformation: nyanya/Infrastructure.Cache.Couchbase/TimelineCache.cs
// CreatedTime: 2014/04/26   4:31 PM
// LastUpdatedTime: 2014/04/29   6:03 PM

using System.Configuration;
using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching.Memcached;

namespace Infrastructure.Cache.Couchbase
{
    public class TimelineCache
    {
        private static readonly CouchbaseClient couchbaseClient;

        static TimelineCache()
        {
            CouchbaseClientSection couchbaseClientSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/Timeline");
            couchbaseClient = new CouchbaseClient(couchbaseClientSection);
        }

        public static object Get(string key)
        {
            return couchbaseClient.Get(key);
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
    }
}