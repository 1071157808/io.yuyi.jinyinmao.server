// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  11:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  11:35 PM
// ***********************************************************************
// <copyright file="RegularProductMap.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Yuyi.Jinyinmao.Domain.Models.Mapping
{
    internal class RegularProductMap : EntityTypeConfiguration<RegularProduct>
    {
        internal RegularProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ProductIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductNo)
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.PledgeNo)
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.Info)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("RegularProducts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductIdentifier).HasColumnName("ProductIdentifier");
            this.Property(t => t.ProductCategory).HasColumnName("ProductCategory");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductNo).HasColumnName("ProductNo");
            this.Property(t => t.IssueNo).HasColumnName("IssueNo");
            this.Property(t => t.PledgeNo).HasColumnName("PledgeNo");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.FinancingSumCount).HasColumnName("FinancingSumCount");
            this.Property(t => t.MaxShareCount).HasColumnName("MaxShareCount");
            this.Property(t => t.MinShareCount).HasColumnName("MinShareCount");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.LaunchTime).HasColumnName("LaunchTime");
            this.Property(t => t.StartSellTime).HasColumnName("StartSellTime");
            this.Property(t => t.EndSellTime).HasColumnName("EndSellTime");
            this.Property(t => t.SoldOut).HasColumnName("SoldOut");
            this.Property(t => t.SoldOutTime).HasColumnName("SoldOutTime");
            this.Property(t => t.ValueDate).HasColumnName("ValueDate");
            this.Property(t => t.ValueDateMode).HasColumnName("ValueDateMode");
            this.Property(t => t.SettleDate).HasColumnName("SettleDate");
            this.Property(t => t.RepaymentDeadline).HasColumnName("RepaymentDeadline");
            this.Property(t => t.Repaid).HasColumnName("Repaid");
            this.Property(t => t.RepaidTime).HasColumnName("RepaidTime");
            this.Property(t => t.Info).HasColumnName("Info");
        }
    }
}
