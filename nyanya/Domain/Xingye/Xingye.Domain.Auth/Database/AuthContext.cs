// FileInformation: nyanya/Xingye.Domain.Auth/AuthContext.cs
// CreatedTime: 2014/09/02   11:12 AM
// LastUpdatedTime: 2014/09/02   2:00 PM

using System.Data.Entity;
using System.Diagnostics;
using Domian.Database;
using Infrastructure.EL.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Xingye.Domain.Auth.Database.Mapping;

namespace Xingye.Domain.Auth.Database
{
    public class AuthContext : DbContextBase
    {
        static AuthContext()
        {
            IDatabaseInitializer<AuthContext> strategy = new NullDatabaseInitializer<AuthContext>();
            System.Data.Entity.Database.SetInitializer(strategy);
        }

        public AuthContext()
            : base("Name=AuthContext")
        {
            this.retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategyFactory.GetSqlDbContextRetryPolicy());
            this.retryPolicy.Retrying += (s, e) =>
                Trace.TraceError("An error occurred in attempt number {1} to access the AuthContext: {0}", e.LastException.Message, e.CurrentRetryCount);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new VeriCodeMap());
        }
    }
}