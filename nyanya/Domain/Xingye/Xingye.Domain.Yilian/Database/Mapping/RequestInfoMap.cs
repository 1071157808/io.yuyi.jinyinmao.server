// FileInformation: nyanya/Xingye.Domain.Yilian/RequestInfoMap.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/12   11:33 AM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Yilian.Models;

namespace Xingye.Domain.Yilian.Database.Mapping
{
    internal class RequestInfoMap : EntityTypeConfiguration<RequestInfo>
    {
        internal RequestInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestIdentifier);

            // Properties
            this.Property(t => t.RequestIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AccountName)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.AccountNo)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.BankName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.IdNo)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.MobileNo)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Province)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(50);

            this.Property(t => t.OrderIdentifier)
                .HasMaxLength(50);

            this.Property(t => t.ProductNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("RequestInfo", "dbo");
            this.Property(t => t.RequestIdentifier).HasColumnName("RequestIdentifier");
            this.Property(t => t.AccountName).HasColumnName("AccountName");
            this.Property(t => t.AccountNo).HasColumnName("AccountNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.IdNo).HasColumnName("IdNo");
            this.Property(t => t.IdType).HasColumnName("IdType");
            this.Property(t => t.MobileNo).HasColumnName("MobileNo");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
        }
    }
}