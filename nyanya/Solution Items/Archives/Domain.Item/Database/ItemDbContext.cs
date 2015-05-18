// FileInformation: nyanya/Domain.Item/ItemDbContext.cs
// CreatedTime: 2014/06/11   3:23 PM
// LastUpdatedTime: 2014/07/31   2:21 AM

using System.Data.Entity;
using System.Diagnostics;
using Cqrs.Domain.Database;
using Domain.Item.Models;
using Domain.Item.Models.Mapping;
using Infrastructure.EL.TransientFaultHandling;
using Infrastructure.EL.TransientFaultHandling.TransientErrorDetectionStrategy;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace Domain.Item.Database
{
    public class ItemDbContext : DbContextBase
    {
        static ItemDbContext()
        {
            IDatabaseInitializer<ItemDbContext> strategy = new NullDatabaseInitializer<ItemDbContext>();
            System.Data.Entity.Database.SetInitializer(strategy);
        }

        public ItemDbContext()
            : base("Name=ItemDbContext")
        {
            this.retryPolicy = new RetryPolicy<MySqlErrorDetectionStrategy>(RetryStrategyFactory.GetMySqlDbContextRetryPolicy());
            this.retryPolicy.Retrying += (s, e) =>
                Trace.TraceError("An error occurred in attempt number {1} to access the ItemDbContext: {0}", e.LastException.Message, e.CurrentRetryCount);
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Models.Item> Items { get; set; }

        public DbSet<OHPItem> OHPItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new OHPItemMap());
            modelBuilder.Configurations.Add(new CategoryMap());
        }
    }
}