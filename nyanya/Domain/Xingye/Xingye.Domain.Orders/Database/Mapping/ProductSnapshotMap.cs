using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xingye.Domain.Orders.Models;

namespace Xingye.Domain.Orders.Database.Mapping
{
    internal class ProductSnapshotMap : EntityTypeConfiguration<ProductSnapshot>
    {
        internal ProductSnapshotMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderIdentifier);

            // Properties
            this.Property(t => t.OrderIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Snapshot)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.OrderIdentifier).HasColumnName("OrderIdentifier");
            this.Property(t => t.Snapshot).HasColumnName("Snapshot");
        }
    }
}