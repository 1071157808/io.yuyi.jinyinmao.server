// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  5:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-29  6:23 PM
// ***********************************************************************
// <copyright file="JBYConfigMap.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Yuyi.Jinyinmao.Domain.Models.Mapping
{
    /// <summary>
    ///     JBYConfigMap.
    /// </summary>
    public class JBYConfigMap : EntityTypeConfiguration<JBYConfig>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JBYConfigMap" /> class.
        /// </summary>
        public JBYConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Notes)
                .IsRequired()
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("JBYConfigs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DateNumber).HasColumnName("DateNumber");
            this.Property(t => t.JBYWithdrawalLimit).HasColumnName("JBYWithdrawalLimit");
            this.Property(t => t.Yield).HasColumnName("Yield");
            this.Property(t => t.Notes).HasColumnName("Notes");
        }
    }
}
