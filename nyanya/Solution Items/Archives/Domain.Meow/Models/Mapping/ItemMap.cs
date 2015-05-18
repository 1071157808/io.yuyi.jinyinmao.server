// FileInformation: nyanya/Domain.Meow/ItemMap.cs
// CreatedTime: 2014/04/22   11:07 AM
// LastUpdatedTime: 2014/05/07   2:03 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Meow.Models.Mapping
{
    /// <summary>
    ///     ItemMap
    /// </summary>
    public class ItemMap : EntityTypeConfiguration<Item>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemMap" /> class.
        /// </summary>
        public ItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Realationship
            this.HasRequired(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(t => t.CategoryId)
                .WillCascadeOnDelete(true);

            // Properties
            this.Property(t => t.CategoryId)
                .IsRequired();

            this.Property(t => t.OwnerGuid)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.IsUsed)
                .IsRequired();

            this.Property(t => t.ReceiveTime)
                .IsRequired();

            this.Property(t => t.Expires)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.OwnerGuid).HasColumnName("OwnerGuid");
            this.Property(t => t.IsUsed).HasColumnName("IsUsed");
            this.Property(t => t.ReceiveTime).HasColumnName("ReceiveTime");
            this.Property(t => t.Expires).HasColumnName("Expires");
            this.Property(t => t.UseTime).HasColumnName("UseTime");

            // Inheritance: Table Per Type
            this.Map(m => m.ToTable("Items", "Meow")).Map<OHPItem>(m => m.ToTable("OHPItems", "Meow"));
        }
    }
}