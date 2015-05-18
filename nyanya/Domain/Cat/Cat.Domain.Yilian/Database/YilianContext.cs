// FileInformation: nyanya/Cat.Domain.Yilian/YilianContext.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/07/27   6:56 PM

using System.Data.Entity;
using System.Diagnostics;
using Cat.Domain.Yilian.Database.Mapping;
using Domian.Database;
using Infrastructure.EL.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Cat.Domain.Yilian.Database
{
    public class YilianContext : DbContextBase
    {
        static YilianContext()
        {
            IDatabaseInitializer<YilianContext> strategy = new NullDatabaseInitializer<YilianContext>();
            System.Data.Entity.Database.SetInitializer(strategy);
        }

        public YilianContext()
            : base("Name=YilianContext")
        {
            this.retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategyFactory.GetSqlDbContextRetryPolicy());
            this.retryPolicy.Retrying += (s, e) =>
                Trace.TraceError("An error occurred in attempt number {1} to access the YilianContext: {0}", e.LastException.Message, e.CurrentRetryCount);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new CallbackInfoMap());
            modelBuilder.Configurations.Add(new GatewayResponseMap());
            modelBuilder.Configurations.Add(new QueryInfoMap());
            modelBuilder.Configurations.Add(new RequestInfoMap());
            modelBuilder.Configurations.Add(new YilianRequestMap());
            modelBuilder.Configurations.Add(new YilianQueryViewMap());
        }
    }
}