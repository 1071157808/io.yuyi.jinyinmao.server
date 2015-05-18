using Domain.Meow.Services;
using Newtonsoft.Json.Linq;

namespace Services.WebAPI.V1.nyanya.App_Config
{
    /// <summary>
    ///    ItemConfig
    /// </summary>
    public class ItemConfig : Config
    {
        /// <summary>
        ///     Gets the banners.
        /// </summary>
        /// <value>
        ///     The banners.
        /// </value>
        public OHPItemConfig OHPItem { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AppBannersConfig" /> class.
        /// </summary>
        public ItemConfig()
        {
            IMeowConfigurationService service = new MeowConfigurationService();
            JObject jObject = service.GetItemConfig();
            this.jsonSettings = jObject;
            this.ConfigSettings();
        }

        /// <summary>
        ///     Configurate the settings.
        /// </summary>
        private void ConfigSettings()
        {
            this.OHPItem = this.jsonSettings.GetValue("OHPItemConfig").ToObject<OHPItemConfig>();
        }

        /// <summary>
        /// OHPItemConfig
        /// </summary>
        public class OHPItemConfig
        {
            /// <summary>
            /// Gets or sets the duration of the validity.
            /// </summary>
            /// <value>
            /// The duration of the validity.
            /// </value>
            public int ValidityDuration { get; set; }
        }
    }
}