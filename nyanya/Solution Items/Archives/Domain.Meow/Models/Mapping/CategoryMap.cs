// FileInformation: nyanya/Domain.Meow/CategoryMap.cs
// CreatedTime: 2014/04/22   5:03 PM
// LastUpdatedTime: 2014/05/07   2:03 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Meow.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Realationship
            this.HasMany(t => t.Items)
                .WithRequired(i => i.Category)
                .HasForeignKey(t => t.CategoryId)
                .WillCascadeOnDelete(true);

            // Properties
            this.Property(t => t.CategoryTitle)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CategoryDescription)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ImageSrc)
                .IsOptional()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("ItemCategories", "Meow");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CategoryTitle).HasColumnName("CategoryTitle");
            this.Property(t => t.CategoryDescription).HasColumnName("CategoryDescription");
            this.Property(t => t.ImageSrc).HasColumnName("ImageSrc");
        }
    }
}