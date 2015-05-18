// FileInformation: nyanya/nyanya.Internal/Startup.cs
// CreatedTime: 2014/08/29   1:56 PM
// LastUpdatedTime: 2014/09/25   5:57 PM

using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.Owin;
using nyanya.Cat;
using nyanya.Internal.Hangfire;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace nyanya.Cat
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
                config.UseAuthorizationFilters(new LocalRequestsOnlyAuthorizationFilter());
            });
            HangfireConfig.Configurate();
        }
    }
}