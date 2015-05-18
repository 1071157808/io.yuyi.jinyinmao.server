// FileInformation: nyanya/Domain.Order/OrderListItemMap.cs
// CreatedTime: 2014/05/13   8:14 AM
// LastUpdatedTime: 2014/05/13   5:31 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Order.Models.Mapping
{
    public class OrderListItemMap : EntityTypeConfiguration<OrderListItem>
    {
        public OrderListItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("net_OrderListItems", "order_production");
            this.Property(t => t.Id).HasColumnName("OrderId");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderNo");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.Status).HasColumnName("OrderStatus");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.Interest).HasColumnName("Interest");
            this.Property(t => t.UserGuid).HasColumnName("UserGuid");
            this.Property(t => t.OrderTime).HasColumnName("OrderTime");
            this.Property(t => t.ExtraInterest).HasColumnName("ExtraInterest");
            this.Property(t => t.SettleDay).HasColumnName("SettleDay");
        }
    }
}