using Cat.Domain.Orders.Models;
using System.Data.Entity.ModelConfiguration;

namespace Cat.Domain.Orders.Database.Mapping
{
    internal class ZCBBillMap : EntityTypeConfiguration<ZCBBill>
    {
        #region Internal Constructors

        internal ZCBBillMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OrderIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ZCBBill", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.RedeemInterest).HasColumnName("RedeemInterest").HasPrecision(18, 4);
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.ResultTime).HasColumnName("ResultTime");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.SN).HasColumnName("SN");
            this.Property(t => t.DelayDate).HasColumnName("DelayDate");
            this.Property(t => t.AgreementName).HasColumnName("AgreementName");
        }

        #endregion Internal Constructors
    }
}