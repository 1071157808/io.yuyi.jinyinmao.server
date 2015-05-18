// FileInformation: nyanya/Cqrs.Domain.Product/ProductContext.cs
// CreatedTime: 2014/07/16   5:00 PM
// LastUpdatedTime: 2014/07/22   5:59 PM

using System.Data.Entity;
using System.Diagnostics;
using Cat.Domain.Products.Database.Mapping;
using Domian.Database;
using Infrastructure.EL.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Cat.Domain.Products.Database
{
    public class ProductContext : DbContextBase
    {
        static ProductContext()
        {
            IDatabaseInitializer<ProductContext> strategy = new NullDatabaseInitializer<ProductContext>();
            System.Data.Entity.Database.SetInitializer(strategy);
        }

        public ProductContext()
            : base("Name=ProductContext")
        {
            this.retryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(RetryStrategyFactory.GetSqlDbContextRetryPolicy());
            this.retryPolicy.Retrying += (s, e) =>
                Trace.TraceError("An error occurred in attempt number {1} to access the ProductContext: {0}", e.LastException.Message, e.CurrentRetryCount);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new BankAcceptanceProductMap());
            modelBuilder.Configurations.Add(new TradeAcceptanceProductMap());
            modelBuilder.Configurations.Add(new AgreementMap());
            modelBuilder.Configurations.Add(new SalePeriodMap());
            modelBuilder.Configurations.Add(new SaleInfoMap());
            modelBuilder.Configurations.Add(new ValueInfoMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new EndorseLinksMap());
            modelBuilder.Configurations.Add(new BAProductInfoMap());
            modelBuilder.Configurations.Add(new TAProductInfoMap());
            modelBuilder.Configurations.Add(new ProductInfoMap());
            modelBuilder.Configurations.Add(new ZCBProductMap());
            modelBuilder.Configurations.Add(new ZCBProductInfoMap());
            modelBuilder.Configurations.Add(new ZCBHistoryMap());
        }
    }
}