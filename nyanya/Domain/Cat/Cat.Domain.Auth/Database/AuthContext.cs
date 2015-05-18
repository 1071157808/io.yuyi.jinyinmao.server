// FileInformation: nyanya/Cat.Domain.Auth/AuthContext.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   4:29 PM

using System.Data.Entity;
using System.Diagnostics;
using Cat.Domain.Auth.Database.Mapping;
using Domian.Database;
using Infrastructure.EL.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Cat.Domain.Auth.Database
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
            modelBuilder.Configurations.Add(new ParameterMap());
        }
    }
}