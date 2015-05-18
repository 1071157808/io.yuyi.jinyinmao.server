// FileInformation: nyanya/Services.WebAPI.V1.nyanya/ApplicationConfig.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/11   11:15 AM

using System;
using Newtonsoft.Json;
using NLog;
using Services.WebAPI.V1.nyanya.App_Config;

namespace Services.WebAPI.V1.nyanya.App_Start
{
    /// <summary>
    ///     ApplicationConfig
    /// </summary>
    public static class ApplicationConfig
    {
        /// <summary>
        ///     Gets the app banners configuration.
        /// </summary>
        /// <value>
        ///     The app banners configuration.
        /// </value>
        public static AppBannersConfig AppBanners { get; private set; }

        /// <summary>
        ///     Gets the application servers configuration.
        /// </summary>
        /// <value>
        ///     The application servers configuration.
        /// </value>
        public static ApplicationServersConfig ApplicationServers { get; private set; }

        /// <summary>
        ///     Gets the application settings.
        /// </summary>
        /// <value>
        ///     The application settings.
        /// </value>
        public static AppSettingsConfig AppSettings { get; private set; }

        /// <summary>
        ///     Gets the banners configuration.
        /// </summary>
        /// <value>
        ///     The banners configuration.
        /// </value>
        public static BannersConfig Banners { get; private set; }

        /// <summary>
        ///     Gets the dev account configuration.
        /// </summary>
        /// <value>
        ///     The dev account configuration.
        /// </value>
        public static DevAccountsConfig DevAccounts { get; private set; }

        /// <summary>
        ///     Initializes the application config.
        /// </summary>
        /// <param name="logger">The logger for the application startup.</param>
        public static void Initialize(Logger logger)
        {
            try
            {
                ApplicationServers = new ApplicationServersConfig();
                logger.Info("ApplicationServersConfig:");
                logger.Info(JsonConvert.SerializeObject(ApplicationServers));
                logger.Info("Timestamp: " + ApplicationServers.GetConfigTimestamp());

                AppBanners = new AppBannersConfig();
                logger.Info("AppBannersConfig:");
                logger.Info(JsonConvert.SerializeObject(AppBanners));
                logger.Info("Timestamp: " + AppBanners.GetConfigTimestamp());

                AppSettings = new AppSettingsConfig();
                logger.Info("AppSettingsConfig:");
                logger.Info(JsonConvert.SerializeObject(AppSettings));
                logger.Info("Timestamp: " + AppSettings.GetConfigTimestamp());

                Banners = new BannersConfig();
                logger.Info("BannersConfig:");
                logger.Info(JsonConvert.SerializeObject(Banners));
                logger.Info("Timestamp: " + Banners.GetConfigTimestamp());

                DevAccounts = new DevAccountsConfig();
                logger.Info("DevAccountsConfig:");
                logger.Info(JsonConvert.SerializeObject(DevAccounts));
                logger.Info("Timestamp: " + DevAccounts.GetConfigTimestamp());
            }
            catch (Exception e)
            {
                logger.Fatal("ApplicationConfigError " + e.Message + e.StackTrace);
            }
        }
    }
}