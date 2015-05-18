// FileInformation: nyanya/Cqrs.Domain.Product/SalePeriodMap.cs
// CreatedTime: 2014/07/16   5:00 PM
// LastUpdatedTime: 2014/07/21   3:25 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.Models;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class SalePeriodMap : EntityTypeConfiguration<SalePeriod>
    {
        internal SalePeriodMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SalePeriod", "dbo");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.StartSellTime).HasColumnName("StartSellTime");
            this.Property(t => t.EndSellTime).HasColumnName("EndSellTime");
            this.Property(t => t.PreStartSellTime).HasColumnName("PreStartSellTime");
            this.Property(t => t.PreEndSellTime).HasColumnName("PreEndSellTime");
        }
    }
}