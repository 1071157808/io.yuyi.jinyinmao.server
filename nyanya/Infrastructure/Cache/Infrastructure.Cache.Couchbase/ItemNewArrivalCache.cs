// FileInformation: nyanya/Infrastructure.Cache.Couchbase/ItemNewArrivalCache.cs
// CreatedTime: 2014/04/23   3:28 PM
// LastUpdatedTime: 2014/04/28   6:02 PM

using System.Configuration;
using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching.Memcached;

namespace Infrastructure.Cache.Couchbase
{
    public class ItemNewArrivalCache
    {
        private static readonly CouchbaseClient couchbaseClient;

        static ItemNewArrivalCache()
        {
            CouchbaseClientSection couchbaseClientSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/ItemNewArrival");
            couchbaseClient = new CouchbaseClient(couchbaseClientSection);
        }

        public static bool CheckNewItemFlag(string key)
        {
            return couchbaseClient.KeyExists(key);
        }

        public static bool RemoveNewItemFlag(string key)
        {
            bool result;
            int times = 0;

            do
            {
                times++;
                result = couchbaseClient.Remove(key);
            } while (!result && times <= 10);

            return result;
        }

        public static bool SetNewItemFlag(string key)
        {
            bool result;
            int times = 0;
            do
            {
                times++;
                result = couchbaseClient.Store(StoreMode.Set, key, "T");
            } while (!result && times <= 10);

            return result;
        }
    }
}