// FileInformation: nyanya/Cqrs.Domain.User/BankCardRecordMap.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/12   1:11 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Users.Models;

namespace Xingye.Domain.Users.Database.Mapping
{
    internal class BankCardRecordMap : EntityTypeConfiguration<BankCardRecord>
    {
        #region Internal Constructors

        internal BankCardRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BankCardNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.BankName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.CityName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SequenceNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Remark)
                .HasMaxLength(200);

            this.Property(t => t.RowVersion)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("BankCardRecords", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.SequenceNo).HasColumnName("SequenceNo");
            this.Property(t => t.Verified).HasColumnName("Verified");
            this.Property(t => t.VerifiedTime).HasColumnName("VerifiedTime");
            this.Property(t => t.VerifingTime).HasColumnName("VerifingTime");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
        }

        #endregion Internal Constructors
    }
}