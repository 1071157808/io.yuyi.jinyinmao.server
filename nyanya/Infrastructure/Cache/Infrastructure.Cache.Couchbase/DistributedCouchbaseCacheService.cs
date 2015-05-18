// FileInformation: nyanya/Infrastructure.Cache.Couchbase/DistributedCouchbaseCacheService.cs
// CreatedTime: 2014/04/23   1:47 PM
// LastUpdatedTime: 2014/04/23   5:06 PM

using System;
using System.Configuration;
using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching.Memcached;
using Infrastructure.Cache.Interface;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Cache.Couchbase
{
    public class DistributedCouchbaseCacheService : IDistributedCacheService
    {
        #region IDistributedCacheService Members

        public JObject Get(string key)
        {
            string[] nodeKey = key.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (nodeKey.Length != 2)
            {
                throw new ArgumentException("Error formate of key");
            }

            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/" + nodeKey[0]);

            CouchbaseClient couchbaseClient = new CouchbaseClient(bucketSection);

            object o = couchbaseClient.Get(nodeKey[1]);
            if (o == null)
            {
                return null;
            }
            return JObject.Parse(o.ToString());
        }

        public bool SetValue(string key, string value)
        {
            string[] nodeKey = key.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (nodeKey.Length != 2)
            {
                throw new ArgumentException("Error formate of key");
            }

            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/" + nodeKey[0]);

            CouchbaseClient couchbaseClient = new CouchbaseClient(bucketSection);

            return couchbaseClient.Store(StoreMode.Set, nodeKey[1], value);
        }

        #endregion IDistributedCacheService Members
    }
}