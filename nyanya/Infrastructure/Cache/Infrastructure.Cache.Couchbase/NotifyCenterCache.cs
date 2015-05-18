// FileInformation: nyanya/Infrastructure.Cache.Couchbase/NotifyCenterCache.cs
// CreatedTime: 2014/06/19   11:25 AM
// LastUpdatedTime: 2014/06/23   10:18 AM

using System.Configuration;
using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching.Memcached;
using Infrastructure.Lib.Extensions;

namespace Infrastructure.Cache.Couchbase
{
    public class NotifyCenterCache
    {
        private static readonly CouchbaseClient couchbaseClient;

        static NotifyCenterCache()
        {
            CouchbaseClientSection couchbaseClientSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/NotifyCenter");
            couchbaseClient = new CouchbaseClient(couchbaseClientSection);
        }

        public static bool Cas(string key, string json, ulong checkValue)
        {
            CasResult<bool> result = couchbaseClient.Cas(StoreMode.Set, key, json, checkValue);
            return result.Result;
        }

        public static string Get(string key)
        {
            return couchbaseClient.Get("NotifyCenter/{0}".FormatWith(key)).ToString();
        }

        public static string Get(string key, out ulong checkValue)
        {
            CasResult<object> value = couchbaseClient.GetWithCas(key);
            checkValue = value.Cas;
            return value.Result.ToString();
        }

        public static bool Set(string key, string json)
        {
            // 避免临时故障
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