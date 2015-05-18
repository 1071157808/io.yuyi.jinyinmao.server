// FileInformation: nyanya/Cqrs.Domain.Order/PaymentInfoMap.cs
// CreatedTime: 2014/08/06   2:40 PM
// LastUpdatedTime: 2014/08/12   12:35 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Orders.Models;

namespace Cat.Domain.Orders.Database.Mapping
{
    internal class PaymentInfoMap : EntityTypeConfiguration<PaymentInfo>
    {
        internal PaymentInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Properties
            this.Property(t => t.OrderIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BankCardNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.BankName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.TransDesc)
                .HasMaxLength(200);

            this.Property(t => t.RowVersion)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("PaymentInfo", "dbo");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.HasResult).HasColumnName("HasResult");
            this.Property(t => t.IsPaid).HasColumnName("IsPaid");
            this.Property(t => t.ResultTime).HasColumnName("ResultTime");
            this.Property(t => t.TransDesc).HasColumnName("TransDesc");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
        }
    }
}