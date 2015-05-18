// FileInformation: nyanya/Domain.Amp/MeowConfigurationMap.cs
// CreatedTime: 2014/03/30   8:08 PM
// LastUpdatedTime: 2014/04/30   4:27 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Amp.Models.Mapping
{
    internal class MeowConfigurationMap : EntityTypeConfiguration<MeowConfiguration>
    {
        internal MeowConfigurationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Key)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("net_MeowConfigurations", "amp_production");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}