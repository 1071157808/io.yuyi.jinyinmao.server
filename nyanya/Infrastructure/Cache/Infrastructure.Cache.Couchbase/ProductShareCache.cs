// FileInformation: nyanya/Infrastructure.Cache.Couchbase/ProductShareCache.cs
// CreatedTime: 2014/08/19   6:41 PM
// LastUpdatedTime: 2014/09/12   2:13 PM

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching.Memcached;
using Enyim.Caching.Memcached.Results;
using Infrastructure.Lib.Extensions;
using Infrastructure.Lib.Logs;
using Infrastructure.Lib.Logs.Implementation;
using Infrastructure.Lib.Utility;
using Infrastructure.SMS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Cache.Couchbase
{
    /// <summary>
    ///     产品销售进度
    /// </summary>
    public static class ProductShareCache
    {
        private static readonly CouchbaseClient couchbaseClient;
        private static readonly ILogger logger = new NLogger("ProductShareCacheLogger");
        private static readonly ISmsAlertsService smsAlertsService = new SmsService();

        static ProductShareCache()
        {
            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/nyanya");
            couchbaseClient = new CouchbaseClient(bucketSection);
        }

        public static ProductShareCacheModel GetShareCache(string productIdentifier)
        {
            Guard.ArgumentNotNullOrEmpty(productIdentifier, "productIdentifier");
            object result = couchbaseClient.GetData("PSC_{0}".FormatWith(productIdentifier));
            return TryGetProductShareCacheModel(result);
        }

        public static IDictionary<string, ProductShareCacheModel> GetShareCaches(IEnumerable<string> productIdentifiers)
        {
            IEnumerable<string> identifiers = productIdentifiers as string[] ?? productIdentifiers.ToArray();
            IList<string> keys = identifiers.Select(i => "PSC_{0}".FormatWith(i)).ToList();
            IDictionary<string, object> results = couchbaseClient.GetData(keys);
            IDictionary<string, ProductShareCacheModel> models = new Dictionary<string, ProductShareCacheModel>();
            foreach (string key in keys)
            {
                ProductShareCacheModel model = TryGetProductShareCacheModel(key, results);
                models.Add(key.Remove(0, 4), model);
            }
            return models;
        }

        public static bool InitShareCache(string productIdentifier, int sumCount)
        {
            Guard.ArgumentNotNullOrEmpty(productIdentifier, "productIdentifier");
            string data = JsonConvert.SerializeObject(new ProductShareCacheModel { Sum = sumCount, Available = sumCount, Paying = 0, Paid = 0 });
            IStoreOperationResult result = couchbaseClient.StoreData(StoreMode.Add, "PSC_{0}".FormatWith(productIdentifier), data);
            if (!result.Success)
            {
                logger.Error("PSC Init Error {0}|{1}|{2}|{3}|{4}".FmtWith(result.StatusCode, result.Message, result.Exception.IfNotNull(e => e.Message), productIdentifier, sumCount));
            }
            return result.Success;
        }

        public static bool Paid(string productIdentifier, int count, out int remain)
        {
            Guard.ArgumentNotNullOrEmpty(productIdentifier, "productIdentifier");
            Guard.ArgumentMustBePositiveValue(count, "count");
            remain = -1;
            bool result = false;
            int i = 0;
            try
            {
                while (i < 1000)
                {
                    logger.Info("Paid {0}|{1}|{2}".FmtWith(productIdentifier, count, i));
                    CasResult<object> getResult;
                    IGetOperationResult tryResult = couchbaseClient.TryGetWithLock("PSC_{0}".FormatWith(productIdentifier), new TimeSpan(0, 0, 0, 3), out getResult);
                    if (tryResult.Success)
                    {
                        if (getResult.StatusCode != 0)
                        {
                            logger.Error("PSC Paid CasGet Error {0}|{1}|{2}|{3}".FmtWith(getResult.StatusCode, getResult.Result.ToString(), productIdentifier, count));
                            break;
                        }

                        JObject data = JObject.Parse(getResult.Result.ToString());
                        int sum = data.GetValue("Sum").Value<int>();
                        int available = data.GetValue("Available").Value<int>();
                        int paying = data.GetValue("Paying").Value<int>();
                        int paid = data.GetValue("Paid").Value<int>();
                        int newPaid = paid + count;
                        int newPaying = paying - count;
                        remain = available + newPaying;

                        string newData = JsonConvert.SerializeObject(new ProductShareCacheModel { Sum = sum, Available = available, Paying = newPaying, Paid = newPaid });
                        IStoreOperationResult storeResult = couchbaseClient.CasData(StoreMode.Replace, "PSC_{0}".FormatWith(productIdentifier), newData, getResult.Cas);
                        if (storeResult.Success)
                        {
                            result = true;
                            logger.Info("PSC Paid {0}|{1}".FmtWith(productIdentifier, count));
                        }
                        else
                        {
                            logger.Error("PSC Paid CasStore Error {0}|{1}|{2}|{3}".FmtWith(getResult.StatusCode, getResult.Result.ToString(), productIdentifier, count));
                        }
                        break;
                    }
                    Thread.Sleep(TimeSpan.FromMilliseconds(1));
                    i++;
                }
            }
            catch (Exception e)
            {
                logger.Error("PSC Paid Unexpected Error {0}|{1}|{2}|{3}".FmtWith(result, e.Message, productIdentifier, count));
                smsAlertsService.AlertAsync("PSC Paid Error {0}|{1}".FmtWith(productIdentifier, count));
            }
            if (!result)
            {
                smsAlertsService.AlertAsync("PSC Paid Failed {0}|{1}".FmtWith(productIdentifier, count));
            }
            return result;
        }

        public static bool Paying(string productIdentifier, int count)
        {
            Guard.ArgumentNotNullOrEmpty(productIdentifier, "productIdentifier");
            bool result = false;
            int i = 0;
            try
            {
                while (i < 1000)
                {
                    logger.Info("Paying {0}|{1}|{2}".FmtWith(productIdentifier, count, i));
                    CasResult<object> getResult;
                    IGetOperationResult tryResult = couchbaseClient.TryGetWithLock("PSC_{0}".FormatWith(productIdentifier), new TimeSpan(0, 0, 0, 2), out getResult);
                    if (tryResult.Success)
                    {
                        if (getResult.StatusCode != 0)
                        {
                            logger.Error("PSC Paying CasGet Error {0}|{1}|{2}|{3}".FmtWith(getResult.StatusCode, getResult.Result.ToString(), productIdentifier, count));
                            break;
                        }

                        JObject data = JObject.Parse(getResult.Result.ToString());
                        int sum = data.GetValue("Sum").Value<int>();
                        int available = data.GetValue("Available").Value<int>();
                        int paying = data.GetValue("Paying").Value<int>();
                        int paid = data.GetValue("Paid").Value<int>();

                        if (available >= count)
                        {
                            int newAvailable = available - count;
                            int newPaying = paying + count;
                            string newData = JsonConvert.SerializeObject(new ProductShareCacheModel { Sum = sum, Available = newAvailable, Paying = newPaying, Paid = paid });
                            IStoreOperationResult storeResult = couchbaseClient.CasData(StoreMode.Replace, "PSC_{0}".FormatWith(productIdentifier), newData, getResult.Cas);
                            if (storeResult.Success)
                            {
                                result = true;
                                logger.Info("PSC Paying {0}|{1}".FmtWith(productIdentifier, count));
                            }
                            else
                            {
                                logger.Error("PSC Paying CasStore Error {0}|{1}|{2}|{3}".FmtWith(getResult.StatusCode, getResult.Result.ToString(), productIdentifier, count));
                            }
                        }
                        break;
                    }
                    Thread.Sleep(TimeSpan.FromMilliseconds(1));
                    i++;
                }
            }
            catch (Exception e)
            {
                logger.Error("PSC Paying Unexpected Error {0}|{1}|{2}|{3}".FmtWith(result, e.Message, productIdentifier, count));
                smsAlertsService.AlertAsync("PSC Paying Error {0}|{1}".FmtWith(productIdentifier, count));
            }
            if (!result && count < 0)
            {
                smsAlertsService.AlertAsync("PSC Paying Failed {0}|{1}".FmtWith(productIdentifier, count));
            }
            return result;
        }

        public static bool RestoreSoldOutShareCache(string productIdentifier, int sumCount)
        {
            Guard.ArgumentNotNullOrEmpty(productIdentifier, "productIdentifier");
            string data = JsonConvert.SerializeObject(new ProductShareCacheModel { Sum = sumCount, Available = 0, Paying = 0, Paid = sumCount });
            IStoreOperationResult result = couchbaseClient.StoreData(StoreMode.Add, "PSC_{0}".FormatWith(productIdentifier), data);
            if (!result.Success)
            {
                logger.Error("PSC Init Error {0}|{1}|{2}|{3}|{4}".FmtWith(result.StatusCode, result.Message, result.Exception.IfNotNull(e => e.Message), productIdentifier, sumCount));
            }
            return result.Success;
        }

        public static bool UpdateShareCache(string productIdentifier, int sumCount)
        {
            Guard.ArgumentNotNullOrEmpty(productIdentifier, "producctIdentifier");
            Guard.ArgumentMustBePositiveValue(sumCount, "sumCount");
            bool result = false;
            try
            {
                logger.Info("UpdateShareCache {0}|{1}".FmtWith(productIdentifier, sumCount));
                CasResult<object> getResult;
                IGetOperationResult tryResult = couchbaseClient.TryGetWithLock("PSC_{0}".FormatWith(productIdentifier), new TimeSpan(0, 0, 0, 3), out getResult);
                if (tryResult.Success)
                {
                    if (getResult.StatusCode != 0)
                    {
                        logger.Error("PSC UpdateShareCache CasGet Error {0}|{1}|{2}|{3}".FmtWith(getResult.StatusCode, getResult.Result.ToString(), productIdentifier, sumCount));
                        return result;
                    }
                    JObject data = JObject.Parse(getResult.Result.ToString());
                    int available = data.GetValue("Available").Value<int>();
                    int paying = data.GetValue("Paying").Value<int>();
                    int paid = data.GetValue("Paid").Value<int>();
                    if (paying != 0)
                    {
                        logger.Error("PSC UpdateShareCache Error {0}|{1}|{2}|{3}".FmtWith(getResult.StatusCode, getResult.Result.ToString(), productIdentifier, sumCount));
                        return result;
                    }
                    string newData = JsonConvert.SerializeObject(new ProductShareCacheModel { Sum = sumCount, Available = sumCount, Paying = 0, Paid = 0 });
                    IStoreOperationResult storeResult = couchbaseClient.CasData(StoreMode.Replace, "PSC_{0}".FmtWith(productIdentifier), newData, getResult.Cas);
                    if (storeResult.Success)
                    {
                        result = true;
                        logger.Info("PSC UpdateShareCache {0}|{1}".FmtWith(productIdentifier, sumCount));
                    }
                    else
                    {
                        logger.Error("PSC UpdateShareCache CasStore Error {0}|{1}|{2}|{3}".FmtWith(getResult.StatusCode, getResult.Result.ToString(), productIdentifier, sumCount));
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("PSC UpdateShare Error {0}|{1}|{2}|{3}".FmtWith(result, e.Message, productIdentifier, sumCount));
            }
            return result;
        }

        private static ProductShareCacheModel TryGetProductShareCacheModel(object value)
        {
            ProductShareCacheModel model;
            if (value != null)
            {
                try
                {
                    model = JsonConvert.DeserializeObject<ProductShareCacheModel>(value.ToString());
                }
                catch
                {
                    model = new ProductShareCacheModel { Available = 0, Paid = 0, Paying = 0, Sum = 0 };
                }
            }
            else
            {
                model = new ProductShareCacheModel { Available = 0, Paid = 0, Paying = 0, Sum = 0 };
            }
            return model;
        }

        private static ProductShareCacheModel TryGetProductShareCacheModel(string key, IDictionary<string, object> results)
        {
            ProductShareCacheModel model;
            object value;
            model = results.TryGetValue(key, out value) ? TryGetProductShareCacheModel(value) : new ProductShareCacheModel { Available = 0, Paid = 0, Paying = 0, Sum = 0 };
            return model;
        }
    }

    public class ProductShareCacheModel
    {
        public int Available { get; set; }

        public int Paid { get; set; }

        public int Paying { get; set; }

        public int Sum { get; set; }
    }
}