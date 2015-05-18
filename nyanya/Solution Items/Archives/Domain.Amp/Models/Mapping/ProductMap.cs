// FileInformation: nyanya/Domain.Amp/ProductMap.cs
// CreatedTime: 2014/03/25   10:04 AM
// LastUpdatedTime: 2014/03/27   4:34 PM

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.Amp.Models.Mapping
{
    internal class ProductMap : EntityTypeConfiguration<Product>
    {
        internal ProductMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductIdentifier)
                .HasMaxLength(32);

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.BankName)
                .HasMaxLength(100);

            this.Property(t => t.IsRecommand)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("net_Products", "amp_production");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.SalesStatus).HasColumnName("SalesStatus");
            this.Property(t => t.TotalNumber).HasColumnName("TotalNumber");
            this.Property(t => t.MinNumber).HasColumnName("MinNumber");
            this.Property(t => t.MaxNumber).HasColumnName("MaxNumber");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.PubBegin).HasColumnName("PubBegin");
            this.Property(t => t.PubEnd).HasColumnName("PubEnd");
            this.Property(t => t.ValueDay).HasColumnName("ValueDay");
            this.Property(t => t.SettleDay).HasColumnName("SettleDay");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.IsRecommand).HasColumnName("IsBest");
            this.Property(t => t.RaiseStatus).HasColumnName("RaiseStatus");
            this.Property(p => p.Unit).HasColumnName("Unit");
        }
    }
}