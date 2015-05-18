// FileInformation: nyanya/Cqrs.Domain.User/BankCardMap.cs
// CreatedTime: 2014/07/28   11:35 AM
// LastUpdatedTime: 2014/08/12   1:10 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Users.Models;

namespace Xingye.Domain.Users.Database.Mapping
{
    internal class BankCardMap : EntityTypeConfiguration<BankCard>
    {
        internal BankCardMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BankCardNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.BankName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CityName)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("BankCards", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.VerifiedTime).HasColumnName("VerifiedTime");
        }
    }
}