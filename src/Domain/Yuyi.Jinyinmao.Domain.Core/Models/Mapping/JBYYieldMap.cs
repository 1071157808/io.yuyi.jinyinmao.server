// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  11:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  11:34 PM
// ***********************************************************************
// <copyright file="JBYYieldMap.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Yuyi.Jinyinmao.Domain.Models.Mapping
{
    internal class JBYYieldMap : EntityTypeConfiguration<JBYYield>
    {
        internal JBYYieldMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Notes)
                .IsRequired()
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("JBYYields");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DateNumber).HasColumnName("DateNumber");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.Notes).HasColumnName("Notes");
        }
    }
}
