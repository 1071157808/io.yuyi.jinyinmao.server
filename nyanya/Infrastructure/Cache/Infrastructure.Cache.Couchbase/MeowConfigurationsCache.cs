// FileInformation: nyanya/Infrastructure.Cache.Couchbase/MeowConfigurationsCache.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/11   11:20 AM

using Couchbase;
using Couchbase.Configuration;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace Infrastructure.Cache.Couchbase
{
    /// <summary>
    ///     MeowConfigurationsCache 主要用于读取配置文件，使用对于性能要求较低，为方便使用，使用静态类实现。
    /// </summary>
    public static class MeowConfigurationsCache
    {
        #region Private Fields

        private static readonly CouchbaseClient couchbaseClient;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes the <see cref="MeowConfigurationsCache" /> class.
        /// </summary>
        static MeowConfigurationsCache()
        {
            CouchbaseClientSection bucketSection = (CouchbaseClientSection)ConfigurationManager.GetSection("couchbase/MeowConfigurations");

            couchbaseClient = new CouchbaseClient(bucketSection);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Gets the app banners.
        /// </summary>
        /// <returns>AppBanners JObject</returns>
        public static JObject GetAppBanners()
        {
            return JObject.Parse(couchbaseClient.Get("AppBanners").ToString());
        }

        /// <summary>
        ///     Gets the application servers.
        /// </summary>
        /// <returns>ApplicationServers JObject</returns>
        public static JObject GetApplicationServers()
        {
            return JObject.Parse(couchbaseClient.Get("ApplicationServers").ToString());
        }

        /// <summary>
        ///     Gets the application settings.
        /// </summary>
        /// <returns>AppSettings JObject</returns>
        public static JObject GetAppSettings()
        {
            return JObject.Parse(couchbaseClient.Get("AppSettings").ToString());
        }

        /// <summary>
        ///     Gets the banners.
        /// </summary>
        /// <returns>Banners JObject</returns>
        public static JObject GetBanners()
        {
            return JObject.Parse(couchbaseClient.Get("Banners").ToString());
        }

        /// <summary>
        ///     Gets the dev accounts.
        /// </summary>
        /// <returns>DevAccounts JObject</returns>
        public static JObject GetDevAccounts()
        {
            return JObject.Parse(couchbaseClient.Get("DevAccounts").ToString());
        }

        #endregion Public Methods
    }
}