using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cat.Domain.Meow.Models;

namespace Cat.Domain.Meow.Database.Mapping
{
    internal class MeowSettingMap : EntityTypeConfiguration<MeowSetting>
    {
        internal MeowSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Key)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(800);

            // Table & Column Mappings
            this.ToTable("Settings", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Value).HasColumnName("Value");
        }
    }
}