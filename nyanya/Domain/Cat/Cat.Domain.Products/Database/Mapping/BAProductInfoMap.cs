// FileInformation: nyanya/Cqrs.Domain.Product/BAProductInfoMap.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/12   12:59 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.ReadModels;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class BAProductInfoMap : EntityTypeConfiguration<BAProductInfo>
    {
        internal BAProductInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Table & Column Mappings
            this.ToTable("BAProductInfo", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.BillNo).HasColumnName("BillNo");
            this.Property(t => t.BusinessLicense).HasColumnName("BusinessLicense");
            this.Property(t => t.ConsignmentAgreementId).HasColumnName("ConsignmentAgreementId");
            this.Property(t => t.EndorseImageLink).HasColumnName("EndorseImageLink");
            this.Property(t => t.EndorseImageThumbnailLink).HasColumnName("EndorseImageThumbnailLink");
            this.Property(t => t.EndSellTime).HasColumnName("EndSellTime");
            this.Property(t => t.EnterpriseName).HasColumnName("EnterpriseName");
            this.Property(t => t.FinancingSum).HasColumnName("FinancingSum");
            this.Property(t => t.FinancingSumCount).HasColumnName("FinancingSumCount");
            this.Property(t => t.LaunchTime).HasColumnName("LaunchTime");
            this.Property(t => t.MaxShareCount).HasColumnName("MaxShareCount");
            this.Property(t => t.MinShareCount).HasColumnName("MinShareCount");
            this.Property(t => t.Period).HasColumnName("Period");
            this.Property(t => t.PledgeAgreementId).HasColumnName("PledgeAgreementId");
            this.Property(t => t.PreEndSellTime).HasColumnName("PreEndSellTime");
            this.Property(t => t.PreStartSellTime).HasColumnName("PreStartSellTime");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.ProductNumber).HasColumnName("ProductNumber");
            this.Property(t => t.ProductType).HasColumnName("ProductType");
            this.Property(t => t.Repaid).HasColumnName("Repaid");
            this.Property(t => t.RepaymentDeadline).HasColumnName("RepaymentDeadline");
            this.Property(t => t.SettleDate).HasColumnName("SettleDate");
            this.Property(t => t.SoldOut).HasColumnName("SoldOut");
            this.Property(t => t.SoldOutTime).HasColumnName("SoldOutTime");
            this.Property(t => t.StartSellTime).HasColumnName("StartSellTime");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.Usage).HasColumnName("Usage");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.ValueDateMode).HasColumnName("ValueDateMode");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.ProductCategory).HasColumnName("ProductCategory");
        }
    }
}