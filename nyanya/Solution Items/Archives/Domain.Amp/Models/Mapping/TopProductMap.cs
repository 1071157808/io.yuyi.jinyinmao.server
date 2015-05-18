// FileInformation: nyanya/Domain.Amp/TopProductMap.cs
// CreatedTime: 2014/03/30   8:08 PM
// LastUpdatedTime: 2014/04/30   4:27 PM

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Amp.Models.Mapping
{
    internal class TopProductMap : EntityTypeConfiguration<TopProduct>
    {
        internal TopProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(32);

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.BankName)
                .HasMaxLength(100);

            this.Property(t => t.IsBest)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("net_TopProducts", "amp_production");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.TotalNumber).HasColumnName("TotalNumber");
            this.Property(t => t.MinNumber).HasColumnName("MinNumber");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.Unit).HasColumnName("Unit");
            this.Property(t => t.IsBest).HasColumnName("IsBest");
        }
    }
}