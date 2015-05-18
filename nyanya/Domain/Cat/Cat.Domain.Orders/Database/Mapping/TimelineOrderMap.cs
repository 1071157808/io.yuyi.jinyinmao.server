// FileInformation: nyanya/Cat.Domain.Orders/TimelineOrderMap.cs
// CreatedTime: 2014/09/15   3:36 PM
// LastUpdatedTime: 2014/09/15   6:23 PM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Orders.Models;

namespace Cat.Domain.Orders.Database.Mapping
{
    internal class TimelineOrderMap : EntityTypeConfiguration<TimelineOrder>
    {
        internal TimelineOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Table & Column Mappings
            this.ToTable("TimelineOrder", "dbo");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.Interest).HasColumnName("Interest");
            this.Property(t => t.OrderTime).HasColumnName("OrderTime");
            this.Property(t => t.OrderType).HasColumnName("OrderType");
            this.Property(t => t.TypeCode).HasColumnName("TypeCode");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.RepaymentDeadline).HasColumnName("RepaymentDeadline");
        }
    }
}