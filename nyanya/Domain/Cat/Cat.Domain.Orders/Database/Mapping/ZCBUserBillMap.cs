using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Orders.Models;

namespace Cat.Domain.Orders.Database.Mapping
{
    internal class ZCBUserBillMap : EntityTypeConfiguration<ZCBUserBill>
    {
        internal ZCBUserBillMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("ZCBUserBill", "dbo");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.BillDate).HasColumnName("BillDate");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.Interest).HasColumnName("Interest").HasPrecision(18,4);
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
