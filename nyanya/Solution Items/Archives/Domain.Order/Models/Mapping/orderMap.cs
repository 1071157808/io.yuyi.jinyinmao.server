// FileInformation: nyanya/Domain.Order/OrderMap.cs
// CreatedTime: 2014/04/01   2:47 PM
// LastUpdatedTime: 2014/04/30   4:26 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Order.Models.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OrderNo)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.UserUuid)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("net_Orders", "order_production");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.OrderNo).HasColumnName("order_no");
            this.Property(t => t.Status).HasColumnName("status");
            this.Property(t => t.Price).HasColumnName("price");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.CreatedAt).HasColumnName("created_at");
            this.Property(t => t.UpdatedAt).HasColumnName("updated_at");
            this.Property(t => t.UserUuid).HasColumnName("user_uuid");
            this.Property(t => t.ExpectedPrice).HasColumnName("expected_price");
        }
    }
}