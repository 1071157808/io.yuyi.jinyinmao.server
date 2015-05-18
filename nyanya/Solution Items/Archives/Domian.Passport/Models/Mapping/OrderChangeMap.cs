// FileInformation: nyanya/Domian.Passport/OrderChangeMap.cs
// CreatedTime: 2014/04/28   3:06 PM
// LastUpdatedTime: 2014/05/04   9:54 AM

using System.Data.Entity.ModelConfiguration;

namespace Domian.Passport.Models.Mapping
{
    public class OrderChangeMap : EntityTypeConfiguration<OrderChange>
    {
        public OrderChangeMap()
        {
            this.HasKey(t => new { t.TriggerId, t.TriggerTypeCode, t.Time });

            // Table & Column Mappings
            this.ToTable("net_OrderChange", "passport_production");
            this.Property(t => t.TriggerId).HasColumnName("TriggerId");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserGuid).HasColumnName("UserGuid");
            this.Property(t => t.TriggerType).HasColumnName("TriggerType");
            this.Property(t => t.TriggerTypeCode).HasColumnName("TriggerTypeCode");
        }
    }
}