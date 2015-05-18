// FileInformation: nyanya/Cqrs.Domain.Order/OrderMap.cs
// CreatedTime: 2014/08/10   1:23 PM
// LastUpdatedTime: 2014/08/16   7:39 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Orders.Models;

namespace Cat.Domain.Orders.Database.Mapping
{
    internal class OrderMap : EntityTypeConfiguration<Order>
    {
        #region Internal Constructors

        internal OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Properties
            this.Property(t => t.OrderIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OrderNo)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.ExtraInterest).HasColumnName("ExtraInterest");
            this.Property(t => t.Interest).HasColumnName("Interest");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.OrderTime).HasColumnName("OrderTime");
            this.Property(t => t.OrderType).HasColumnName("OrderType");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.SettleDate).HasColumnName("SettleDate");
            this.Property(t => t.ShareCount).HasColumnName("ShareCount");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.Yield).HasColumnName("Yield");

            //Relationship
            this.HasRequired(t => t.InvestorInfo).WithRequiredPrincipal();
            this.HasRequired(t => t.PaymentInfo).WithRequiredPrincipal();
            this.HasRequired(t => t.ProductInfo).WithRequiredPrincipal();
            this.HasOptional(t => t.AgreementsInfo).WithRequired();
            this.HasOptional(t => t.ProductSnapshot).WithRequired();
        }

        #endregion Internal Constructors
    }
}