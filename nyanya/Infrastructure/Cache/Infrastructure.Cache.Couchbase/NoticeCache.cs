// FileInformation: nyanya/Infrastructure.Cache.Couchbase/NoticeCache.cs
// CreatedTime: 2014/09/11   14:41 PM
// LastUpdatedTime: 2014/09/11   14:41 PM

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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Cache.Couchbase
{
    /// <summary>
    ///     公告
    /// </summary>
    public static class NoticeCache
    {
        private static readonly CouchbaseClientSection bucketSection;
        private static readonly CouchbaseClient couchbaseClient;
        private static readonly ILogger logger = new NLogger("NoticeCacheLogger");

        static NoticeCache()
        {
            bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/notice");
            couchbaseClient = new CouchbaseClient(bucketSection);
        }

        public static NoticeCacheModel GetNoticeCache()
        {
            object result = couchbaseClient.Get("PublicNotice");
            return TryGetNoticeCacheModel(result);
        }

        public static bool SetNoticeCache(NoticeCacheModel data)
        {
            IStoreOperationResult result = couchbaseClient.ExecuteStore(StoreMode.Set, "PublicNotice", JsonConvert.SerializeObject(data));
            if (!result.Success)
            {
                logger.Error("PublicNotice Init Error {0}|{1}|{2}".FmtWith(result.StatusCode, result.Message, result.Exception.IfNotNull(e => e.Message)));
            }
            return result.Success;
        }
        
        private static NoticeCacheModel TryGetNoticeCacheModel(object value)
        {
            NoticeCacheModel model;
            if (value != null)
            {
                try
                {
                    model = JsonConvert.DeserializeObject<NoticeCacheModel>(value.ToString());
                }
                catch
                {
                    model = new NoticeCacheModel { Content = "", ExpireTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"), Flag = false };
                }
            }
            else
            {
                model = new NoticeCacheModel { Content = "", ExpireTime = DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"), Flag = false };
            }
            return model;
        }
    }

    public class NoticeCacheModel
    {
        public string Content { get; set; }

        public string ExpireTime { get; set; }

        public bool Flag { get; set; }
    }
}