// FileInformation: nyanya/Cqrs.Domain.Product/SaleInfoMap.cs
// CreatedTime: 2014/07/27   9:04 PM
// LastUpdatedTime: 2014/08/12   12:48 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.Models;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class SaleInfoMap : EntityTypeConfiguration<SaleInfo>
    {
        internal SaleInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SaleInfo", "dbo");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.FinancingSumCount).HasColumnName("FinancingSumCount");
            this.Property(t => t.MaxShareCount).HasColumnName("MaxShareCount");
            this.Property(t => t.MinShareCount).HasColumnName("MinShareCount");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
        }
    }
}