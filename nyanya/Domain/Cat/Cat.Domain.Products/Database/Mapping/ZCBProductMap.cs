using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.Models;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class ZCBProductMap : EntityTypeConfiguration<ZCBProduct>
    {
        internal ZCBProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);
            // Properties
            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.Property(t => t.EnableSale).HasColumnName("EnableSale");
            this.Property(t => t.TotalSaleAmount).HasColumnName("TotalSaleAmount");
            this.Property(t => t.TotalInterest).HasColumnName("TotalInterest");
            this.Property(t => t.TotalRedeemAmount).HasColumnName("TotalRedeemAmount");
            this.Property(t => t.TotalRedeemInterest).HasColumnName("TotalRedeemInterest");
            this.Property(t => t.SubProductNo).HasColumnName("SubProductNo");
            this.Property(t => t.PerRemainRedeemAmount).HasColumnName("PerRemainRedeemAmount");
            this.Property(t => t.ConsignmentAgreementName).HasColumnName("ConsignmentAgreementName");
            this.Property(t => t.PledgeAgreementName).HasColumnName("PledgeAgreementName");

            this.HasMany(t => t.ZCBHistorys).WithRequired().HasForeignKey(b => b.ProductIdentifier);
        }
    }
}
