// FileInformation: nyanya/Cqrs.Domain.Order/AgreementsInfoMap.cs
// CreatedTime: 2014/07/28   11:36 AM
// LastUpdatedTime: 2014/07/28   6:32 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Orders.Models;

namespace Xingye.Domain.Orders.Database.Mapping
{
    internal class AgreementsInfoMap : EntityTypeConfiguration<AgreementsInfo>
    {
        internal AgreementsInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Properties
            this.Property(t => t.OrderIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ConsignmentAgreementContent)
                .IsRequired();

            this.Property(t => t.ConsignmentAgreementName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.PledgeAgreementContent)
                .IsRequired();

            this.Property(t => t.PledgeAgreementName)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("AgreementsInfo", "dbo");
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.ConsignmentAgreementContent).HasColumnName("ConsignmentAgreementContent");
            this.Property(t => t.ConsignmentAgreementName).HasColumnName("ConsignmentAgreementName");
            this.Property(t => t.PledgeAgreementContent).HasColumnName("PledgeAgreementContent");
            this.Property(t => t.PledgeAgreementName).HasColumnName("PledgeAgreementName");
        }
    }
}