// FileInformation: nyanya/Domain.Meow/OrderStatisticMap.cs
// CreatedTime: 2014/04/21   1:12 PM
// LastUpdatedTime: 2014/04/21   4:29 PM

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Meow.Models.Mapping
{
    public class OrderStatisticMap : EntityTypeConfiguration<OrderStatistic>
    {
        public OrderStatisticMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OrderId, t.UserId, t.OrderStatus });

            // Properties
            this.Property(t => t.OrderId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserGuid)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("OrderStatistic", "Meow");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserGuid).HasColumnName("UserGuid");
            this.Property(t => t.Interest).HasColumnName("Interest");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.InterestAccruingBeginningDay).HasColumnName("PubBegin");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.OrderStatus).HasColumnName("OrderStatus");
            this.Property(t => t.ProductStatus).HasColumnName("ProductStatus");
            this.Property(t => t.SettleDay).HasColumnName("SettleDay");
            this.Property(t => t.ExtraInterest).HasColumnName("ExtraInterest");
            this.Property(t => t.OrderTime).HasColumnName("OrderTime");
        }
    }
}