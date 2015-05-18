// FileInformation: nyanya/Cqrs.Domain.Product/BankAcceptanceProductMap.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   12:45 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.Models;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class BankAcceptanceProductMap : EntityTypeConfiguration<BankAcceptanceProduct>
    {
        internal BankAcceptanceProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            this.Property(t => t.BankName)
                .HasMaxLength(100)
                .IsRequired();

            this.Property(t => t.BillNo)
                .HasMaxLength(80)
                .IsRequired();

            this.Property(t => t.BusinessLicense)
                .HasMaxLength(80)
                .IsRequired();

            this.Property(t => t.EnterpriseName)
                .HasMaxLength(80)
                .IsRequired();

            this.Property(t => t.Usage)
                .HasMaxLength(200)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.BillNo).HasColumnName("BillNo");
            this.Property(t => t.BusinessLicense).HasColumnName("BusinessLicense");
            this.Property(t => t.EnterpriseName).HasColumnName("EnterpriseName");
            this.Property(t => t.Usage).HasColumnName("Usage");
        }
    }
}