// FileInformation: nyanya/Domain.Meow/OHPItemMap.cs
// CreatedTime: 2014/04/22   11:55 AM
// LastUpdatedTime: 2014/05/05   2:29 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Meow.Models.Mapping
{
    internal class OHPItemMap : EntityTypeConfiguration<OHPItem>
    {
        internal OHPItemMap()
        {
            // Properties

            // Table & Column Mappings
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.ExtraInterest).HasColumnName("ExtraInterest");
        }
    }
}