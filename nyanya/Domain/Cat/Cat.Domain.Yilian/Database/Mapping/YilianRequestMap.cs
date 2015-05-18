// FileInformation: nyanya/Cat.Domain.Yilian/YilianRequestMap.cs
// CreatedTime: 2014/07/30   5:40 PM
// LastUpdatedTime: 2014/08/08   10:54 AM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Yilian.Models;

namespace Cat.Domain.Yilian.Database.Mapping
{
    public class YilianRequestMap : EntityTypeConfiguration<YilianRequest>
    {
        internal YilianRequestMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestIdentifier);

            // Properties
            this.Property(t => t.RequestIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SequenceNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.TypeCode)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.Property(t => t.RequestIdentifier).HasColumnName("RequestIdentifier");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.SequenceNo).HasColumnName("SequenceNo");
            this.Property(t => t.TypeCode).HasColumnName("TypeCode");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.IsPayment).HasColumnName("IsPayment");

            //Relationship
            this.HasRequired(t => t.RequestInfo).WithRequiredPrincipal();
            this.HasOptional(t => t.CallbackInfo).WithRequired();
            this.HasOptional(t => t.GatewayResponse).WithRequired();
            this.HasOptional(t => t.QueryInfo).WithRequired();

            // Inheritance: Table Per Hierarchy
            this.Map(m => m.ToTable("YilianRequests", "dbo"));
        }
    }
}