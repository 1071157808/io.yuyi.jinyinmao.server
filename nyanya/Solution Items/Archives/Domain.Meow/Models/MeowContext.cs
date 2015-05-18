// FileInformation: nyanya/Domain.Meow/MeowContext.cs
// CreatedTime: 2014/03/30   10:12 PM
// LastUpdatedTime: 2014/04/23   2:44 PM

using System.Data.Entity;
using Domain.Meow.Models.Mapping;

namespace Domain.Meow.Models
{
    public class MeowContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<OHPItem> OHPItems { get; set; }

        /// <summary>
        ///     Gets or sets the order statistics. From view, readonly.
        /// </summary>
        /// <value>
        ///     The order statistics.
        /// </value>
        public DbSet<OrderStatistic> OrderStatistics { get; set; }

        static MeowContext()
        {
            Database.SetInitializer<MeowContext>(null);
        }

        public MeowContext()
            : base("Name=MeowContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FeedbackMap());
            modelBuilder.Configurations.Add(new ItemMap());
            modelBuilder.Configurations.Add(new OrderStatisticMap());
            modelBuilder.Configurations.Add(new OHPItemMap());
            modelBuilder.Configurations.Add(new CategoryMap());
        }
    }
}