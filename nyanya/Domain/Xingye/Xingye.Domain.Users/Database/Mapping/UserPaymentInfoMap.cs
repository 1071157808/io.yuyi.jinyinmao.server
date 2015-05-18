// FileInformation: nyanya/Cqrs.Domain.User/UserPaymentInfoMap.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   1:15 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Users.Models;

namespace Xingye.Domain.Users.Database.Mapping
{
    internal class UserPaymentInfoMap : EntityTypeConfiguration<UserPaymentInfo>
    {
        internal UserPaymentInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UserIdentifier);

            // Properties
            this.Property(t => t.EncryptedPassword)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.Salt)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("UserPaymentInfo", "dbo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.EncryptedPassword).HasColumnName("EncryptedPassword");
            this.Property(t => t.FailedCount).HasColumnName("FailedCount");
            this.Property(t => t.LastFailedTime).HasColumnName("LastFailedTime");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.SetTime).HasColumnName("SetTime");
        }
    }
}