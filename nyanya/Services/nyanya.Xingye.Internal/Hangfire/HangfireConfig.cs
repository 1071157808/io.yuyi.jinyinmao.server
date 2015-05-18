// FileInformation: nyanya/nyanya.Xingye.Internal/HangfireConfig.cs
// CreatedTime: 2014/08/29   11:57 AM
// LastUpdatedTime: 2014/08/29   11:58 AM

using Hangfire;

namespace nyanya.Xingye.Internal.Hangfire
{
    /// <summary>
    ///     HangfireConfig
    /// </summary>
    public static class HangfireConfig
    {
        /// <summary>
        ///     Configurates this instance.
        /// </summary>
        public static void Configurate()
        {
            RecurringJob.AddOrUpdate<CronJobs>(j => j.SetProductSoldOut(), Cron.Minutely);
            RecurringJob.AddOrUpdate<CronJobs>(j => j.VerifyOrder(), Cron.Minutely);
            RecurringJob.AddOrUpdate<CronJobs>(j => j.VerifyYilianAuth(), Cron.Minutely);
        }
    }
}