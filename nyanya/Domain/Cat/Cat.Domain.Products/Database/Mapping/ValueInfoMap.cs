// FileInformation: nyanya/Cqrs.Domain.Product/ValueInfoMap.cs
// CreatedTime: 2014/07/27   9:04 PM
// LastUpdatedTime: 2014/08/12   12:49 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.Models;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class ValueInfoMap : EntityTypeConfiguration<ValueInfo>
    {
        internal ValueInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ValueInfo", "dbo");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.RepaymentDeadline).HasColumnName("RepaymentDeadline");
            this.Property(t => t.SettleDate).HasColumnName("SettleDate");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.ValueDateMode).HasColumnName("ValueDateMode");
        }
    }
}