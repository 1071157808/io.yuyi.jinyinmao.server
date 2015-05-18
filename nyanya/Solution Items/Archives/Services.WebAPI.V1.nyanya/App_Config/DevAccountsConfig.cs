// FileInformation: nyanya/Services.WebAPI.V1.nyanya/DevAccountsConfig.cs
// CreatedTime: 2014/05/22   1:56 AM
// LastUpdatedTime: 2014/08/11   10:27 AM

using System.Collections.Generic;
using Infrastructure.Cache.Couchbase;
using Newtonsoft.Json.Linq;

namespace Services.WebAPI.V1.nyanya.App_Config
{
    /// <summary>
    ///     DevAccountsConfig
    /// </summary>
    public class DevAccountsConfig : Config
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DevAccountsConfig" /> class.
        /// </summary>
        public DevAccountsConfig()
        {
            JObject jObject = MeowConfigurationsCache.GetDevAccounts();
            this.jsonSettings = jObject;
            this.ConfigSettings();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     Gets the administrators.
        /// </summary>
        /// <value>
        ///     The administrators.
        /// </value>
        public IEnumerable<string> Administrators { get; private set; }

        /// <summary>
        ///     Gets the developers.
        /// </summary>
        /// <value>
        ///     The developers.
        /// </value>
        public IEnumerable<string> Developers { get; private set; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        ///     Configurate the settings.
        /// </summary>
        private void ConfigSettings()
        {
            this.Administrators = this.jsonSettings.GetValue("Administrators").ToObject<IEnumerable<string>>();
            this.Developers = this.jsonSettings.GetValue("Developers").ToObject<IEnumerable<string>>();
        }

        #endregion Private Methods
    }
}