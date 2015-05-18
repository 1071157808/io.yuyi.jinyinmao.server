// FileInformation: nyanya/Cat.Domain.Products/TAProductInfoMap.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/17   11:56 AM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.ReadModels;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class TAProductInfoMap : EntityTypeConfiguration<TAProductInfo>
    {
        internal TAProductInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Table & Column Mappings
            this.ToTable("TAProductInfo", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.BillNo).HasColumnName("BillNo");
            this.Property(t => t.ConsignmentAgreementId).HasColumnName("ConsignmentAgreementId");
            this.Property(t => t.ConsignmentAgreementName).HasColumnName("ConsignmentAgreementName");
            this.Property(t => t.Drawee).HasColumnName("Drawee");
            this.Property(t => t.DraweeInfo).HasColumnName("DraweeInfo");
            this.Property(t => t.EndorseImageLink).HasColumnName("EndorseImageLink");
            this.Property(t => t.EndorseImageThumbnailLink).HasColumnName("EndorseImageThumbnailLink");
            this.Property(t => t.EnterpriseInfo).HasColumnName("EnterpriseInfo");
            this.Property(t => t.EnterpriseLicense).HasColumnName("EnterpriseLicense");
            this.Property(t => t.EnterpriseName).HasColumnName("EnterpriseName");
            this.Property(t => t.EndSellTime).HasColumnName("EndSellTime");
            this.Property(t => t.FinancingSum).HasColumnName("FinancingSum");
            this.Property(t => t.FinancingSumCount).HasColumnName("FinancingSumCount");
            this.Property(t => t.GuaranteeMode).HasColumnName("GuaranteeMode");
            this.Property(t => t.LaunchTime).HasColumnName("LaunchTime");
            this.Property(t => t.MaxShareCount).HasColumnName("MaxShareCount");
            this.Property(t => t.MinShareCount).HasColumnName("MinShareCount");
            this.Property(t => t.Period).HasColumnName("Period");
            this.Property(t => t.PledgeAgreementId).HasColumnName("PledgeAgreementId");
            this.Property(t => t.PledgeAgreementName).HasColumnName("PledgeAgreementName");
            this.Property(t => t.PreEndSellTime).HasColumnName("PreEndSellTime");
            this.Property(t => t.PreStartSellTime).HasColumnName("PreStartSellTime");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.ProductNumber).HasColumnName("ProductNumber");
            this.Property(t => t.ProductType).HasColumnName("ProductType");
            this.Property(t => t.Repaid).HasColumnName("Repaid");
            this.Property(t => t.Securedparty).HasColumnName("Securedparty");
            this.Property(t => t.SecuredpartyInfo).HasColumnName("SecuredpartyInfo");
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