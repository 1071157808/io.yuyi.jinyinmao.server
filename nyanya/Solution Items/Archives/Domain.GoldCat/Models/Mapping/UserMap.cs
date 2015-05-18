// FileInformation: nyanya/Domain.GoldCat/UserMap.cs
// CreatedTime: 2014/04/04   9:45 AM
// LastUpdatedTime: 2014/04/04   9:54 AM

using System.Data.Entity.ModelConfiguration;

namespace Domain.GoldCat.Models.Mapping
{
    public class GoldCatUserMap : EntityTypeConfiguration<GoldCatUser>
    {
        public GoldCatUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Cellphone)
                .HasMaxLength(11);

            this.Property(t => t.Name)
                .HasMaxLength(30);

            this.Property(t => t.RealName)
                .HasMaxLength(30);

            this.Property(t => t.IdCard)
                .HasMaxLength(18);

            this.Property(t => t.EncryptedPassword)
                .HasMaxLength(80);

            this.Property(t => t.Salt)
                .HasMaxLength(64);

            // Table & Column Mappings
            this.ToTable("users", "gold_cat_production");
            this.Property(t => t.Guid).HasColumnName("uuid");
            this.Property(t => t.Cellphone).HasColumnName("cellphone");
            this.Property(t => t.Name).HasColumnName("name");
            this.Property(t => t.RealName).HasColumnName("real_name");
            this.Property(t => t.IdCard).HasColumnName("id_card");
            this.Property(t => t.EncryptedPassword).HasColumnName("encrypted_password");
            this.Property(t => t.Salt).HasColumnName("salt");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.CreatedAt).HasColumnName("created_at");
            this.Property(t => t.UpdatedAt).HasColumnName("updated_at");
        }
    }
}