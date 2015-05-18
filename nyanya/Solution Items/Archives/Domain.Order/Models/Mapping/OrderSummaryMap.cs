// FileInformation: nyanya/Domain.Order/OrderSummaryMap.cs
// CreatedTime: 2014/04/01   2:47 PM
// LastUpdatedTime: 2014/05/08   1:08 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Order.Models.Mapping
{
    public class OrderSummaryMap : EntityTypeConfiguration<OrderSummary>
    {
        public OrderSummaryMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OrderId, t.OrderNo, t.OrderStatus, t.UserId });

            // Properties
            this.Property(t => t.OrderId);

            this.Property(t => t.OrderNo)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.UserId);

            this.Property(t => t.UserGuid)
                .HasMaxLength(80);

            this.Property(t => t.ProductIdentifer)
                .HasMaxLength(32);

            this.Property(t => t.ProductInfo)
                .HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("net_OrderSummaries", "order_production");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.OrderStatus).HasColumnName("OrderStatus");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserGuid).HasColumnName("UserGuid");
            this.Property(t => t.ProductIdentifer).HasColumnName("ProductIdentifer");
            this.Property(t => t.ExpectedYield).HasColumnName("ExpectedYield");
            this.Property(t => t.ProductInfo).HasColumnName("ProductInfo");
            this.Property(t => t.ExpectedPrice).HasColumnName("ExpectedPrice");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}