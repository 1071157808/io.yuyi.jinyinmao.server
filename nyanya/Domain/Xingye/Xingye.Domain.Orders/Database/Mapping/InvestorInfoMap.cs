// FileInformation: nyanya/Cqrs.Domain.Order/InvestorInfoMap.cs
// CreatedTime: 2014/07/29   10:36 AM
// LastUpdatedTime: 2014/08/12   11:05 AM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Orders.Models;

namespace Xingye.Domain.Orders.Database.Mapping
{
    internal class InvestorInfoMap : EntityTypeConfiguration<InvestorInfo>
    {
        internal InvestorInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Properties
            this.Property(t => t.OrderIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.CredentialNo)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RealName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("InvestorInfo", "dbo");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.Credential).HasColumnName("Credential");
            this.Property(t => t.CredentialNo).HasColumnName("CredentialNo");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
        }
    }
}