using System.Collections.Generic;
using System.Configuration;
using Couchbase;
using Couchbase.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Cache.Couchbase
{
    public static class YinlianErrorCodeCache
    {
        private static readonly CouchbaseClient CouchbaseClient;
        private static Dictionary<string, string> ErrorMessageList { get; set; }

        static YinlianErrorCodeCache()
        {
            var bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/notice");
            CouchbaseClient = new CouchbaseClient(bucketSection);
        }

        public static Dictionary<string, string> GetErrorMessageList()
        {
            var result = CouchbaseClient.Get("YilianErrorMessages");
            if (ErrorMessageList == null)
            {
                try
                {
                    ErrorMessageList = JsonConvert.DeserializeObject<Dictionary<string, string>>(result.ToString());
                }
                catch
                {
                    ErrorMessageList = new Dictionary<string, string>();
                }
            }
            return ErrorMessageList;
        }

        public static string TryGetErrorMessage(string errorCode)
        {
            if (string.IsNullOrEmpty(errorCode))
                return string.Empty;

            string errorMessage;

            var flg = GetErrorMessageList().TryGetValue(errorCode,out errorMessage);

            return flg ? errorMessage : "";
        }
    }
}
