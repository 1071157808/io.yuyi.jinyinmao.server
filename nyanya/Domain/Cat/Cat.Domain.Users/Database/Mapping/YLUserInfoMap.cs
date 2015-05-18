// FileInformation: nyanya/Cqrs.Domain.User/YLUserInfoMap.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   1:16 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Users.Models;

namespace Cat.Domain.Users.Database.Mapping
{
    internal class YLUserInfoMap : EntityTypeConfiguration<YLUserInfo>
    {
        internal YLUserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UserIdentifier);

            // Properties
            this.Property(t => t.CredentialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RealName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.RowVersion)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("YLUserInfo", "dbo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.Credential).HasColumnName("Credential");
            this.Property(t => t.CredentialNo).HasColumnName("CredentialNo");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.VerifiedTime).HasColumnName("VerifiedTime");
            this.Property(t => t.VerifingTime).HasColumnName("VerifingTime");
            this.Property(t => t.Verified).HasColumnName("Verified");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
        }
    }
}