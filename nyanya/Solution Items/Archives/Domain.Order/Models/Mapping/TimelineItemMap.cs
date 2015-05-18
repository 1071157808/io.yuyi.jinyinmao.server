// FileInformation: nyanya/Domain.Order/TimelineItemMap.cs
// CreatedTime: 2014/04/01   7:28 PM
// LastUpdatedTime: 2014/04/01   7:30 PM

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Order.Models.Mapping
{
    public class TimelineItemMap : EntityTypeConfiguration<TimelineItem>
    {
        public TimelineItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Type);

            // Properties
            this.Property(t => t.Type)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Identifier)
                .HasMaxLength(32);

            this.Property(t => t.UserGuid)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("net_TimelineItems", "order_production");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Identifier).HasColumnName("Identifier");
            this.Property(t => t.Interest).HasColumnName("Interest");
            this.Property(t => t.Principal).HasColumnName("Principal");
            this.Property(t => t.UserGuid).HasColumnName("UserGuid");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}