// FileInformation: nyanya/Cqrs.Domain.User/UserMap.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   1:15 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Users.Models;

namespace Xingye.Domain.Users.Database.Mapping
{
    internal class UserMap : EntityTypeConfiguration<User>
    {
        internal UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserIdentifier);

            // Properties
            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.IdentificationCode)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.RowVersion)
                .IsRowVersion();

            this.Property(t => t.UserType).IsRequired();

            // Table & Column Mappings
            this.ToTable("Users", "dbo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.IdentificationCode).HasColumnName("IdentificationCode");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
            this.Property(t => t.UserType).HasColumnName("UserType");

            //Relationship
            this.HasRequired(t => t.UserLoginInfo).WithRequiredPrincipal();
            this.HasMany(t => t.BankCards).WithRequired().HasForeignKey(b => b.UserIdentifier);
            this.HasOptional(t => t.YSBUserInfo).WithRequired();
            this.HasOptional(t => t.YLUserInfo).WithRequired();
            this.HasOptional(t => t.UserPaymentInfo).WithRequired();
        }
    }
}