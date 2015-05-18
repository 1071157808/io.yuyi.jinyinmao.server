using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching.Memcached;
using Enyim.Caching.Memcached.Results;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Utility;
using Newtonsoft.Json;

namespace Infrastructure.Cache.Couchbase
{
    public static class ParameterCache
    {
        private static readonly CouchbaseClient couchbaseClient;
        static ParameterCache()
        {
            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/nyanya");
            couchbaseClient = new CouchbaseClient(bucketSection);
        }

        public static ParameterCacheModel GetByName(string parameterName)
        {
            try
            {
                Guard.ArgumentNotNullOrEmpty(parameterName, "parameterName");
                object result = couchbaseClient.GetData("Param_{0}".FormatWith(parameterName));
                return TryGetParameterCacheModel(result);
            }
            catch
            {
                return null;
            }
        }

        public static bool SetCache(string parameterName, string parameterValue)
        {
            try
            {
                Guard.ArgumentNotNullOrEmpty(parameterName, "parameterName");
                string data = JsonConvert.SerializeObject(new ParameterCacheModel { Name = parameterName, Value = parameterValue });
                IStoreOperationResult result = couchbaseClient.StoreData(StoreMode.Add, "Param_{0}".FormatWith(parameterName), data);
                return result.Success;
            }
            catch
            {
                return false;
            }
        }

        private static ParameterCacheModel TryGetParameterCacheModel(object value)
        {
            if (value == null) return null;
            try
            {
                return JsonConvert.DeserializeObject<ParameterCacheModel>(value.ToString());
            }
            catch
            {
                return null;
            }
        }
        
    }

    public class ParameterCacheModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
