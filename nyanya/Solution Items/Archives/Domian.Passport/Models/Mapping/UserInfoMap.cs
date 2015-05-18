// FileInformation: nyanya/Domian.Passport/UserInfoMap.cs
// CreatedTime: 2014/03/31   3:59 PM
// LastUpdatedTime: 2014/04/02   7:55 PM

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Passport.Models.Mapping
{
    public class UserInfoMap : EntityTypeConfiguration<UserInfo>
    {
        public UserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Cellphone)
                .HasMaxLength(15);

            this.Property(t => t.Password)
                .HasMaxLength(80);

            this.Property(t => t.Salt)
                .HasMaxLength(80);

            this.Property(t => t.Guid)
                .HasMaxLength(80);

            this.Property(t => t.IdCard)
                .HasMaxLength(255);

            this.Property(t => t.RealName)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("net_UserInfo", "passport_production");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.Guid).HasColumnName("Guid");
            this.Property(t => t.FailuresCount).HasColumnName("FailuresCount");
            this.Property(t => t.FailedAt).HasColumnName("FailedAt");
            this.Property(t => t.IdCard).HasColumnName("IdCard");
            this.Property(t => t.RealName).HasColumnName("Name");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}