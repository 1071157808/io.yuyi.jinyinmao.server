// FileInformation: nyanya/Xingye.Domain.Meow/MeowContext.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   2:00 PM

using System.Data.Entity;
using System.Diagnostics;
using Domian.Database;
using Infrastructure.EL.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Xingye.Domain.Meow.Database.Mapping;

namespace Xingye.Domain.Meow.Database
{
    public class MeowContext : DbContextBase
    {
        static MeowContext()
        {
            IDatabaseInitializer<MeowContext> strategy = new NullDatabaseInitializer<MeowContext>();
            System.Data.Entity.Database.SetInitializer(strategy);
        }

        public MeowContext()
            : base("Name=MeowContext")
        {
            this.retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategyFactory.GetSqlDbContextRetryPolicy());
            this.retryPolicy.Retrying += (s, e) =>
                Trace.TraceError("An error occurred in attempt number {1} to access the MeowContext: {0}", e.LastException.Message, e.CurrentRetryCount);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new FeedbackMap());
            modelBuilder.Configurations.Add(new MeowSettingMap());
        }
    }
}