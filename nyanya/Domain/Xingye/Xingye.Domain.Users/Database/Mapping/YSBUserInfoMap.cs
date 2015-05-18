// FileInformation: nyanya/Cqrs.Domain.User/YSBUserInfoMap.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   1:17 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Users.Models;

namespace Xingye.Domain.Users.Database.Mapping
{
    internal class YSBUserInfoMap : EntityTypeConfiguration<YSBUserInfo>
    {
        internal YSBUserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UserIdentifier);

            // Properties
            this.Property(t => t.IdCard)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.RealName)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("YSBUserInfo", "dbo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.IdCard).HasColumnName("IdCard");
            this.Property(t => t.RealName).HasColumnName("RealName");
        }
    }
}