// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ApplicationServersConfig.cs
// CreatedTime: 2014/04/20   8:20 PM
// LastUpdatedTime: 2014/04/21   9:24 PM

using System.Collections.Generic;
using System.Linq;
using Infrastructure.Cache.Couchbase;
using Newtonsoft.Json.Linq;

namespace Services.WebAPI.V1.nyanya.App_Config
{
    /// <summary>
    ///     ApplicationServersConfig
    /// </summary>
    public class ApplicationServersConfig : Config
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApplicationServersConfig" /> class.
        /// </summary>
        public ApplicationServersConfig()
        {
            JObject jObject = MeowConfigurationsCache.GetApplicationServers();
            this.jsonSettings = jObject;
            this.ConfigSettings();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     Gets the master server.
        /// </summary>
        /// <value>
        ///     The master server.
        /// </value>
        public string MasterServer
        {
            get { return this.Servers.FirstOrDefault(); }
        }

        /// <summary>
        ///     Gets the servers.
        /// </summary>
        /// <value>
        ///     The servers.
        /// </value>
        public IList<string> Servers { get; private set; }

        /// <summary>
        ///     Gets the servers count.
        /// </summary>
        /// <value>
        ///     The servers count.
        /// </value>
        public int ServersCount
        {
            get { return this.Servers.Count; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        ///     Configurate the settings.
        /// </summary>
        private void ConfigSettings()
        {
            this.Servers = this.jsonSettings.GetValue("Servers").ToObject<IList<string>>();
        }

        #endregion Private Methods
    }
}