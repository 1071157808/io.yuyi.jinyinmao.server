// FileInformation: nyanya/Cqrs.Domain.User/UserLoginInfoMap.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/12   1:19 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Users.Models;

namespace Cat.Domain.Users.Database.Mapping
{
    internal class UserLoginInfoMap : EntityTypeConfiguration<UserLoginInfo>
    {
        internal UserLoginInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UserIdentifier);

            // Properties
            this.Property(t => t.EncryptedPassword)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.LoginName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Salt)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("UserLoginInfo", "dbo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.EncryptedPassword).HasColumnName("EncryptedPassword");
            this.Property(t => t.LastFailedSignInTime).HasColumnName("LastFailedSignInTime");
            this.Property(t => t.LoginFailedCount).HasColumnName("LoginFailedCount");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.SignUpTime).HasColumnName("SignUpTime");
            this.Property(t => t.LastSuccessSignInTime).HasColumnName("LastSuccessSignInTime");
        }
    }
}