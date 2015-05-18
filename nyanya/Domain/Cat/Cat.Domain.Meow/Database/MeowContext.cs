// FileInformation: nyanya/Cat.Domain.Meow/MeowContext.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System.Data.Entity;
using System.Diagnostics;
using Cat.Domain.Meow.Database.Mapping;
using Domian.Database;
using Infrastructure.EL.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Cat.Domain.Meow.Database
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