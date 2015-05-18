// FileInformation: nyanya/Cqrs.Domain.Product/ProductMap.cs
// CreatedTime: 2014/07/30   5:41 PM
// LastUpdatedTime: 2014/08/12   12:46 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Products.Models;

namespace Xingye.Domain.Products.Database.Mapping
{
    internal class ProductMap : EntityTypeConfiguration<Product>
    {
        #region Internal Constructors

        internal ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            this.Property(t => t.ProductName)
                .HasMaxLength(50)
                .IsRequired();

            this.Property(t => t.ProductNo)
                .HasMaxLength(40)
                .IsRequired();

            this.Property(t => t.Yield)
                .HasPrecision(5, 3);

            // Table & Column Mappings
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ConsignmentAgreementId).HasColumnName("ConsignmentAgreementId");
            this.Property(t => t.LaunchTime).HasColumnName("LaunchTime");
            this.Property(t => t.Period).HasColumnName("Period");
            this.Property(t => t.PledgeAgreementId).HasColumnName("PledgeAgreementId");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.ProductNumber).HasColumnName("ProductNumber");
            this.Property(t => t.ProductType).HasColumnName("ProductType");
            this.Property(t => t.Repaid).HasColumnName("Repaid");
            this.Property(t => t.SoldOut).HasColumnName("SoldOut");
            this.Property(t => t.SoldOutTime).HasColumnName("SoldOutTime");
            this.Property(t => t.Yield).HasColumnName("Yield");

            // Relationship
            this.HasRequired(t => t.ConsignmentAgreement).WithMany().HasForeignKey(t => t.ConsignmentAgreementId);
            this.HasRequired(t => t.Links).WithRequiredPrincipal().WillCascadeOnDelete(true);
            this.HasRequired(t => t.PledgeAgreement).WithMany().HasForeignKey(t => t.PledgeAgreementId);
            this.HasRequired(t => t.SaleInfo).WithRequiredPrincipal().WillCascadeOnDelete(true);
            this.HasRequired(t => t.SalePeriod).WithRequiredPrincipal().WillCascadeOnDelete(true);
            this.HasRequired(t => t.ValueInfo).WithRequiredPrincipal().WillCascadeOnDelete(true);

            // Inheritance: Table Per Type
            this.Map(m => m.ToTable("Products", "dbo")).Map<BankAcceptanceProduct>(m => m.ToTable("BankAcceptanceProducts", "dbo")).Map<TradeAcceptanceProduct>(m => m.ToTable("TradeAcceptanceProducts", "dbo"));
        }

        #endregion Internal Constructors
    }
}