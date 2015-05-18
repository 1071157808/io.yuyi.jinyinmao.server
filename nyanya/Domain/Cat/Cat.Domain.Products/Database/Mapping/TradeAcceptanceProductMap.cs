// FileInformation: nyanya/Cat.Domain.Products/TradeAcceptanceProductMap.cs
// CreatedTime: 2014/09/01   2:44 PM
// LastUpdatedTime: 2014/09/17   11:53 AM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.Models;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class TradeAcceptanceProductMap : EntityTypeConfiguration<TradeAcceptanceProduct>
    {
        internal TradeAcceptanceProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            this.Property(t => t.BillNo)
                .HasMaxLength(80)
                .IsRequired();

            this.Property(t => t.ConsignmentAgreementName)
                .HasMaxLength(80)
                .IsRequired();

            this.Property(t => t.Drawee)
                .HasMaxLength(300)
                .IsRequired();

            this.Property(t => t.DraweeInfo)
                .HasMaxLength(1000)
                .IsRequired();

            this.Property(t => t.EnterpriseInfo)
                .HasMaxLength(1000)
                .IsRequired();

            this.Property(t => t.EnterpriseLicense)
                .HasMaxLength(80)
                .IsRequired();

            this.Property(t => t.EnterpriseName)
                .HasMaxLength(300)
                .IsRequired();

            this.Property(t => t.PledgeAgreementName)
                .HasMaxLength(80)
                .IsRequired();

            this.Property(t => t.Securedparty)
                .HasMaxLength(300)
                .IsRequired();

            this.Property(t => t.SecuredpartyInfo)
                .HasMaxLength(1000)
                .IsRequired();

            this.Property(t => t.Usage)
                .HasMaxLength(1000)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.BillNo).HasColumnName("BillNo");
            this.Property(t => t.ConsignmentAgreementName).HasColumnName("ConsignmentAgreementName");
            this.Property(t => t.Drawee).HasColumnName("Drawee");
            this.Property(t => t.DraweeInfo).HasColumnName("DraweeInfo");
            this.Property(t => t.EnterpriseInfo).HasColumnName("EnterpriseInfo");
            this.Property(t => t.EnterpriseLicense).HasColumnName("EnterpriseLicense");
            this.Property(t => t.EnterpriseName).HasColumnName("EnterpriseName");
            this.Property(t => t.GuaranteeMode).HasColumnName("GuaranteeMode");
            this.Property(t => t.PledgeAgreementName).HasColumnName("PledgeAgreementName");
            this.Property(t => t.Securedparty).HasColumnName("Securedparty");
            this.Property(t => t.SecuredpartyInfo).HasColumnName("SecuredpartyInfo");
            this.Property(t => t.Usage).HasColumnName("Usage");
        }
    }
}