// FileInformation: nyanya/Infrastructure.Cache.Couchbase/MeowConfigurationsCache.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/11   11:20 AM

using Couchbase;
using Couchbase.Configuration;
using Newtonsoft.Json;
using System.Configuration;

namespace Infrastructure.Cache.Couchbase
{
    public static class MeowUpgradeCache
    {
        private static readonly CouchbaseClient couchbaseClient;

        static MeowUpgradeCache()
        {
            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/upgrade");
            couchbaseClient = new CouchbaseClient(bucketSection);
            int a = 1;
        }

        public static MeowUpgradeCacheModel GetMeowUpgradeCache(string channel)
        {
            object result = couchbaseClient.Get(channel);
            return TryGetMeowUpgradeCacheModel(result);
        }

        private static MeowUpgradeCacheModel TryGetMeowUpgradeCacheModel(object value)
        {
            MeowUpgradeCacheModel model;
            if (value != null)
            {
                try
                {
                    model = JsonConvert.DeserializeObject<MeowUpgradeCacheModel>(value.ToString());
                }
                catch
                {
                    model = null;
                }
            }
            else
            {
                model = null;
            }
            return model;
        }
    }

    public class MeowUpgradeCacheModel
    {
        public string Channel { get; set; }

        public string Source { get; set; }

        public string MaxVersion { get; set; }

        public string MustVersion { get; set; }

        public string Url { get; set; }

        public string Message { get; set; }
    }
}