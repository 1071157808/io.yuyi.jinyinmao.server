// FileInformation: nyanya/Infrastructure.Cache.Runtime/RuntimeCache.cs
// CreatedTime: 2014/04/23   8:52 AM
// LastUpdatedTime: 2014/04/23   5:48 PM

using System;
using System.Web;
using Infrastructure.Cache.Couchbase;
using Infrastructure.Cache.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Cache.Runtime
{
    public class RuntimeCache
    {
        private readonly IDistributedCacheService distributedCacheService;

        public RuntimeCache(IDistributedCacheService distributedCacheService)
        {
            this.distributedCacheService = distributedCacheService;
        }

        public RuntimeCache()
        {
            this.distributedCacheService = new DistributedCouchbaseCacheService();
        }

        public T GetValue<T>(string key) where T : class
        {
            T t;
            if (this.GetValueFromHttpRuntime(key, out t))
            {
                return t;
            }

            if (this.GetValueFromDistributedCache(key, out t))
            {
                return t;
            }

            return null;
        }

        public bool SetValue(string key, object value)
        {
            int times = 0;
            bool result = false;
            JObject json = new JObject();

            if (value is ValueType || value is String)
            {
                json.Add("json", new JValue(value));
            }
            else
            {
                json.Add("json", JObject.FromObject(value));
            }

            string jsonValue = JsonConvert.SerializeObject(json);

            while (!result && times <= 10)
            {
                result = this.distributedCacheService.SetValue(key, jsonValue);
                times++;
            }

            if (result)
            {
                this.RestoreValue(key, value);
            }

            return result;
        }

        private bool GetValueFromDistributedCache<T>(string key, out T t) where T : class
        {
            t = default(T);
            JObject json = this.distributedCacheService.Get(key);
            if (json == null)
            {
                return false;
            }

            T value = json.GetValue("json").ToObject<T>();

            if (value == null)
            {
                return false;
            }

            t = value;
            this.RestoreValue(key, value);
            return true;
        }

        private bool GetValueFromHttpRuntime<T>(string key, out T t) where T : class
        {
            t = default(T);
            T value = HttpRuntime.Cache.Get(key) as T;
            if (value == null)
            {
                return false;
            }

            t = value;
            return true;
        }

        private void RestoreValue(string key, object value)
        {
            HttpRuntime.Cache.Insert(key, value);
        }
    }
}