// FileInformation: nyanya/Domain.Order/OrderWithPIMap.cs
// CreatedTime: 2014/05/13   8:14 AM
// LastUpdatedTime: 2014/05/13   4:05 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Order.Models.Mapping
{
    internal class OrderWithPIMap : EntityTypeConfiguration<OrderWithPI>
    {
        internal OrderWithPIMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderId);

            // Table & Column Mappings
            this.ToTable("net_OrderWithPI", "order_production");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.ExtraInterest).HasColumnName("ExtraInterest");
            this.Property(t => t.UserGuid).HasColumnName("UserGuid");
            this.Property(t => t.SettleDay).HasColumnName("SettleDay");
        }
    }
}