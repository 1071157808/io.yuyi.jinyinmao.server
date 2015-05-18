// FileInformation: nyanya/Cqrs.Domain.Order/ProductInfoMap.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/12   12:37 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Orders.Models;

namespace Xingye.Domain.Orders.Database.Mapping
{
    internal class ProductInfoMap : EntityTypeConfiguration<ProductInfo>
    {
        internal ProductInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Properties
            this.Property(t => t.OrderIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EndorseImageLink)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.EndorseImageThumbnailLink)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.ProductIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductNo)
                .IsRequired()
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("ProductInfo", "dbo");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.EndorseImageLink).HasColumnName("EndorseImageLink");
            this.Property(t => t.EndorseImageThumbnailLink).HasColumnName("EndorseImageThumbnailLink");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.ProductNumber).HasColumnName("ProductNumber");
            this.Property(t => t.ProductYield).HasColumnName("ProductYield");
            this.Property(t => t.RepaymentDeadline).HasColumnName("RepaymentDeadline");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
        }
    }
}