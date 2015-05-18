using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.Models;

namespace Cat.Domain.Products.Database.Mapping
{
    public class ZCBHistoryMap: EntityTypeConfiguration<ZCBHistory>
    {
        public ZCBHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ZCBHistory", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.FinancingSumCount).HasColumnName("FinancingSumCount");
            this.Property(t => t.SubProductNo).HasColumnName("SubProductNo");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.NextStartSellTime).HasColumnName("NextStartSellTime");
            this.Property(t => t.NextEndSellTime).HasColumnName("NextEndSellTime");
            this.Property(t => t.NextYield).HasColumnName("NextYield");
            this.Property(t => t.EnableSale).HasColumnName("EnableSale");
            this.Property(t => t.PledgeAgreementName).HasColumnName("PledgeAgreementName");
            this.Property(t => t.PledgeAgreement).HasColumnName("PledgeAgreement");
            this.Property(t => t.ConsignmentAgreementName).HasColumnName("ConsignmentAgreementName");
            this.Property(t => t.ConsignmentAgreement).HasColumnName("ConsignmentAgreement");
            this.Property(t => t.PerRemainRedeemAmount).HasColumnName("PerRemainRedeemAmount");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
        
    }
}
