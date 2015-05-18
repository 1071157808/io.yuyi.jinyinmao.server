using Cat.Domain.Orders.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.Domain.Orders.Database.Mapping
{
    internal class ZCBUserMap : EntityTypeConfiguration<ZCBUser>
    {
        internal ZCBUserMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserIdentifier, t.ProductIdentifier });

            this.ToTable("ZCBUser", "dbo");
            this.Property(t => t.CurrentPrincipal).HasColumnName("CurrentPrincipal");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.TotalInterest).HasColumnName("TotalInterest").HasPrecision(18, 4);
            this.Property(t => t.TotalPrincipal).HasColumnName("TotalPrincipal");
            this.Property(t => t.TotalRedeemInterest).HasColumnName("TotalRedeemInterest").HasPrecision(18, 4);
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.YesterdayInterest).HasColumnName("YesterdayInterest").HasPrecision(18, 4);
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
        }
    }
}