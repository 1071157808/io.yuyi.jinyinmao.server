// FileInformation: nyanya/Cat.Domain.Auth/VeriCodeMap.cs
// CreatedTime: 2014/07/04   1:43 PM
// LastUpdatedTime: 2014/07/09   9:40 AM

using System.Data.Entity.ModelConfiguration;
using Cat.Domain.Auth.Models;

namespace Cat.Domain.Auth.Database.Mapping
{
    internal class VeriCodeMap : EntityTypeConfiguration<VeriCode>
    {
        internal VeriCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Identifier);

            // Properties
            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(60);

            this.Property(t => t.RowVersion)
                .IsRowVersion();

            // Table & Column Mappings
            this.ToTable("VeriCodes", "dbo");
            this.Property(t => t.BuildAt).HasColumnName("BuildAt");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ErrorCount).HasColumnName("ErrorCount");
            this.Property(t => t.Identifier).HasColumnName("Identifier");
            this.Property(t => t.RowVersion).HasColumnName("RowVersion");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Times).HasColumnName("Times");
            this.Property(t => t.Used).HasColumnName("Used");
            this.Property(t => t.Verified).HasColumnName("Verified");
            this.Property(t => t.ClientId).HasColumnName("ClientId");
        }
    }
}