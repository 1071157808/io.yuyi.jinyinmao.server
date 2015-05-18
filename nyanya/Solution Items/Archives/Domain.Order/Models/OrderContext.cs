// FileInformation: nyanya/Domain.Order/OrderContext.cs
// CreatedTime: 2014/04/01   2:47 PM
// LastUpdatedTime: 2014/05/07   5:48 PM

using System.Data.Entity;
using Domain.Order.Models.Mapping;

namespace Domain.Order.Models
{
    public class OrderContext : DbContext
    {
        static OrderContext()
        {
            Database.SetInitializer<OrderContext>(null);
        }

        public OrderContext()
            : base("Name=OrderContext")
        {
        }

        /// <summary>
        ///     Gets or sets the order list items. From view, readonly.
        /// </summary>
        /// <value>
        ///     The order list items.
        /// </value>
        public DbSet<OrderListItem> OrderListItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        /// <summary>
        ///     Gets or sets the order summaries. From view, readonly.
        /// </summary>
        /// <value>
        ///     The order summaries.
        /// </value>
        public DbSet<OrderSummary> OrderSummaries { get; set; }

        /// <summary>
        ///     Gets or sets the order with p is.
        /// </summary>
        /// <value>
        ///     The order with p is.
        /// </value>
        public DbSet<OrderWithPI> OrderWithPIs { get; set; }

        /// <summary>
        ///     Gets or sets the settle orders.
        /// </summary>
        /// <value>
        ///     The settle orders.
        /// </value>
        public DbSet<SettleOrder> SettleOrders { get; set; }

        /// <summary>
        ///     Gets or sets the timeline items. From view, readonly.
        /// </summary>
        /// <value>
        ///     The timeline items.
        /// </value>
        public DbSet<TimelineItem> TimelineItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderListItemMap());
            modelBuilder.Configurations.Add(new OrderSummaryMap());
            modelBuilder.Configurations.Add(new TimelineItemMap());
            modelBuilder.Configurations.Add(new SettleOrderMap());
            modelBuilder.Configurations.Add(new OrderWithPIMap());
        }
    }
}