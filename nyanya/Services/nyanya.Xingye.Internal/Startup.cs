// FileInformation: nyanya/nyanya.Xingye.Internal/Startup.cs
// CreatedTime: 2014/09/03   9:48 AM
// LastUpdatedTime: 2014/09/03   6:32 PM

using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using nyanya.Xingye.Internal;
using nyanya.Xingye.Internal.Hangfire;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace nyanya.Xingye.Internal
{
    /// <summary>
    ///     Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfire(config =>
            {
                config.UseSqlServerStorage("Hangfire");
                config.UseServer("critical", "default");
                //config.UseAuthorizationFilters(new DashboardAuthorizationFilter());
            });
            HangfireConfig.Configurate();
        }
    }
}