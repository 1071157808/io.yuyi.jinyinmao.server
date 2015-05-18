// FileInformation: nyanya/Cqrs.Domain.Product/EndorseLinksMap.cs
// CreatedTime: 2014/07/21   4:56 PM
// LastUpdatedTime: 2014/07/21   5:00 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Products.Models;

namespace Cat.Domain.Products.Database.Mapping
{
    internal class EndorseLinksMap : EntityTypeConfiguration<EndorseLinks>
    {
        internal EndorseLinksMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            this.Property(t => t.EndorseImageLink)
                .HasMaxLength(300)
                .IsRequired();

            this.Property(t => t.EndorseImageThumbnailLink)
                .HasMaxLength(300)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("EndorseLinks", "dbo");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.EndorseImageLink).HasColumnName("EndorseImageLink");
            this.Property(t => t.EndorseImageThumbnailLink).HasColumnName("EndorseImageThumbnailLink");
        }
    }
}