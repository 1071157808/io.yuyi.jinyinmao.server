// FileInformation: nyanya/Domain.Scheduler/InvestmentStatisticMap.cs
// CreatedTime: 2014/04/28   5:42 PM
// LastUpdatedTime: 2014/05/04   1:06 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Scheduler.Models.Mapping
{
    public class InvestmentStatisticMap : EntityTypeConfiguration<InvestmentStatistic>
    {
        public InvestmentStatisticMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(36);

            // Table & Column Mappings
            this.ToTable("InvestmentStatistics", "Scheduler");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.AccruedEarnings).HasColumnName("AccruedEarnings");
            this.Property(t => t.InterestPerSecond).HasColumnName("InterestPerSecond");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.OrderCount).HasColumnName("OrderCount");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}