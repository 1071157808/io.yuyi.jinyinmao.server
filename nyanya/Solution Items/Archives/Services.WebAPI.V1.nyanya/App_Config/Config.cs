// FileInformation: nyanya/Services.WebAPI.V1.nyanya/Config.cs
// CreatedTime: 2014/04/20   9:46 PM
// LastUpdatedTime: 2014/04/21   5:00 PM

using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;

namespace Services.WebAPI.V1.nyanya.App_Config
{
    /// <summary>
    ///     Config
    /// </summary>
    public abstract class Config
    {
        /// <summary>
        ///     The json format of settings
        /// </summary>
        protected JObject jsonSettings;

        /// <summary>
        ///     Gets the configuration timestamp.
        /// </summary>
        /// <returns>ConfigTimestamp</returns>
        public string GetConfigTimestamp()
        {
            return this.jsonSettings.GetValue("Timestamp").IfNotNull(j => j.Value<string>());
        }
    }
}