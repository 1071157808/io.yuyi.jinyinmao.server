// FileInformation: nyanya/Cqrs.Domain.User/UserContext.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/08   12:16 PM

using System.Data.Entity;
using System.Diagnostics;
using Cat.Domain.Users.Database.Mapping;
using Domian.Database;
using Infrastructure.EL.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Cat.Domain.Users.Database
{
    public class UserContext : DbContextBase
    {
        static UserContext()
        {
            IDatabaseInitializer<UserContext> strategy = new NullDatabaseInitializer<UserContext>();
            System.Data.Entity.Database.SetInitializer(strategy);
        }

        public UserContext()
            : base("Name=UserContext")
        {
            this.retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategyFactory.GetSqlDbContextRetryPolicy());
            this.retryPolicy.Retrying += (s, e) =>
                Trace.TraceError("An error occurred in attempt number {1} to access the UserContext: {0}", e.LastException.Message, e.CurrentRetryCount);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new BankCardMap());
            modelBuilder.Configurations.Add(new BankCardRecordMap());
            modelBuilder.Configurations.Add(new UserInfoMap());
            modelBuilder.Configurations.Add(new UserLoginInfoMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserPaymentInfoMap());
            modelBuilder.Configurations.Add(new YLUserInfoMap());
            modelBuilder.Configurations.Add(new YSBUserInfoMap());
            modelBuilder.Configurations.Add(new PaymentBankCardInfoMap());
        }
    }
}