// FileInformation: nyanya/Cqrs.Domain.Order/SettlingOrderMap.cs
// CreatedTime: 2014/08/16   12:10 PM
// LastUpdatedTime: 2014/08/16   12:35 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Orders.ReadModels;

namespace Cat.Domain.Orders.Database.Mapping
{
    internal class SettlingOrderMap : EntityTypeConfiguration<SettlingOrder>
    {
        #region Internal Constructors

        internal SettlingOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductIdentifier);
            this.HasKey(t => t.UserIdentifier);

            // Table & Column Mappings
            this.ToTable("SettlingOrders", "dbo");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ExtraInterest).HasColumnName("ExtraInterest");
            this.Property(t => t.Interest).HasColumnName("Interest");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.SettleDate).HasColumnName("SettleDate");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.ProductNumber).HasColumnName("ProductNumber");
            this.Property(t => t.OrderType).HasColumnName("OrderType");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.ProductCategory).HasColumnName("ProductCategory");
        }

        #endregion Internal Constructors
    }
}