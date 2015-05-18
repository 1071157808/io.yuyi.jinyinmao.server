// FileInformation: nyanya/Services.WebAPI.V1.nyanya/AppSettingsConfig.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/11   9:39 AM

using System;
using Infrastructure.Cache.Couchbase;
using Newtonsoft.Json.Linq;

namespace Services.WebAPI.V1.nyanya.App_Config
{
    /// <summary>
    ///     AppSettingsConfig
    /// </summary>
    public class AppSettingsConfig : Config
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppSettingsConfig" /> class.
        /// </summary>
        public AppSettingsConfig()
        {
            JObject jObject = MeowConfigurationsCache.GetAppSettings();
            this.jsonSettings = jObject;
            this.ConfigSettings();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     Gets the duration of the cookie validity.
        /// </summary>
        /// <value>
        ///     The duration of the cookie validity.
        /// </value>
        public int CookieValidityDuration { get; private set; }

        /// <summary>
        ///     Gets or sets the environment.
        /// </summary>
        /// <value>
        ///     The environment.
        /// </value>
        public string Environment { get; set; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        ///     Configurate the settings.
        /// </summary>
        private void ConfigSettings()
        {
            this.CookieValidityDuration = this.jsonSettings.GetValue("CookieValidityDuration").ToObject<Int32>();
            this.Environment = this.jsonSettings.GetValue("Environment").ToObject<string>();
        }

        #endregion Private Methods
    }
}