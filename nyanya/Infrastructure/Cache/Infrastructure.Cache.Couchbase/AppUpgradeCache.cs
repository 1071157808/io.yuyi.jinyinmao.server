using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Configuration;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Cache.Couchbase
{
    public class AppUpgradeCache
    {
        private static readonly CouchbaseClient couchbaseClient;

        /// <summary>
        /// Initializes the <see cref="AppUpgradeCache"/> class.
        /// </summary>
        static AppUpgradeCache()
        {
            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/MeowConfigurations");

            couchbaseClient = new CouchbaseClient(bucketSection);
        }

        public static JObject Get(string clientType)
        {
            return JObject.Parse(couchbaseClient.Get("AppUpgrade_" + clientType).ToString());
        }
    }
}
