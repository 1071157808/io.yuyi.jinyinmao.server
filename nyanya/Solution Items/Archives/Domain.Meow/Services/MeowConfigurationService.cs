// FileInformation: nyanya/Domain.Meow/MeowConfigurationService.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/11   11:15 AM

using Infrastructure.Cache.Couchbase;
using Newtonsoft.Json.Linq;

namespace Domain.Meow.Services
{
    /// <summary>
    ///     MeowConfigurationService
    /// </summary>
    public class MeowConfigurationService : IMeowConfigurationService
    {
        #region IMeowConfigurationService Members

        /// <summary>
        ///     Gets the app banners.
        /// </summary>
        /// <returns>AppBanners JObject</returns>
        public JObject GetAppBanners()
        {
            return MeowConfigurationsCache.GetAppBanners();
        }

        /// <summary>
        ///     Gets the application servers.
        /// </summary>
        /// <returns>ApplicationServers JObject</returns>
        public JObject GetApplicationServers()
        {
            return MeowConfigurationsCache.GetApplicationServers();
        }

        /// <summary>
        ///     Gets the application settings.
        /// </summary>
        /// <returns>AppSettings JObject</returns>
        public JObject GetAppSettings()
        {
            return MeowConfigurationsCache.GetAppSettings();
        }

        /// <summary>
        ///     Gets the banners.
        /// </summary>
        /// <returns>Banners JObject</returns>
        public JObject GetBanners()
        {
            return MeowConfigurationsCache.GetBanners();
        }

        /// <summary>
        ///     Gets the dev accounts.
        /// </summary>
        /// <returns>DevAccounts JObject</returns>
        public JObject GetDevAccounts()
        {
            return MeowConfigurationsCache.GetDevAccounts();
        }

        #endregion IMeowConfigurationService Members
    }
}