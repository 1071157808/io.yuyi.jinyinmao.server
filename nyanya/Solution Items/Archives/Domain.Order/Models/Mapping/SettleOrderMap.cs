// FileInformation: nyanya/Domain.Order/SettleOrderMap.cs
// CreatedTime: 2014/04/30   4:52 PM
// LastUpdatedTime: 2014/05/08   1:15 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Order.Models.Mapping
{
    internal class SettleOrderMap : EntityTypeConfiguration<SettleOrder>
    {
        internal SettleOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserGuid)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("net_SettleOrders", "order_production");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.SettleTime).HasColumnName("SettleTime");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserGuid).HasColumnName("UserGuid");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}