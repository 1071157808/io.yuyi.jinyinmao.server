// FileInformation: nyanya/Infrastructure.Cache.Couchbase/PassportCache.cs
// CreatedTime: 2014/03/31   5:46 PM
// LastUpdatedTime: 2014/04/24   2:09 PM

using System;
using System.Configuration;
using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching.Memcached;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Cache.Couchbase
{
    public class PassportCache
    {
        private static readonly CouchbaseClient couchbaseClient;

        static PassportCache()
        {
            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/Passport");

            couchbaseClient = new CouchbaseClient(bucketSection);
        }

        public JObject GetTokenInfo(string token)
        {
            return JObject.Parse(couchbaseClient.Get(token).ToString());
        }

        public void RemoveToken(string token)
        {
            if (!couchbaseClient.Remove(token))
            {
                throw new Exception("Can not remove token " + token);
            }
        }

        public bool SaveToken(string token, string tokenContent)
        {
            return couchbaseClient.Store(StoreMode.Set, token, tokenContent);
        }

        public bool ValidateToken(string token)
        {
            return couchbaseClient.KeyExists(token);
        }
    }
}