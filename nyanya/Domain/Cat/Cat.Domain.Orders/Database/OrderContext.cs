// FileInformation: nyanya/Cat.Domain.Orders/OrderContext.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/01   3:48 PM

using Cat.Domain.Orders.Database.Mapping;
using Cat.Domain.Orders.Models;
using Domian.Database;
using Infrastructure.EL.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System.Data.Entity;
using System.Diagnostics;

namespace Cat.Domain.Orders.Database
{
    public class OrderContext : DbContextBase
    {
        static OrderContext()
        {
            IDatabaseInitializer<OrderContext> strategy = new NullDatabaseInitializer<OrderContext>();
            System.Data.Entity.Database.SetInitializer(strategy);
        }

        public OrderContext()
            : base("Name=OrderContext")
        {
            this.retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategyFactory.GetSqlDbContextRetryPolicy());
            this.retryPolicy.Retrying += (s, e) =>
                Trace.TraceError("An error occurred in attempt number {1} to access the OrderContext: {0}", e.LastException.Message, e.CurrentRetryCount);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new AgreementsInfoMap());
            modelBuilder.Configurations.Add(new InvestorInfoMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new PaymentInfoMap());
            modelBuilder.Configurations.Add(new ProductInfoMap());
            modelBuilder.Configurations.Add(new ProductSnapshotMap());
            modelBuilder.Configurations.Add(new OrderInfoMap());
            modelBuilder.Configurations.Add(new BAOrderInfoMap());
            modelBuilder.Configurations.Add(new TAOrderInfoMap());
            modelBuilder.Configurations.Add(new SettlingOrderMap());
            modelBuilder.Configurations.Add(new ZCBBillMap());
            modelBuilder.Configurations.Add(new ZCBUserMap());
            modelBuilder.Configurations.Add(new ZCBUserBillMap());
        }
    }
}