// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-07-26  2:23 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-26  2:27 PM
// ***********************************************************************
// <copyright file="PrincipalCouponMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Yuyi.Jinyinmao.Domain.Models.Mapping
{
    /// <summary>
    ///     PrincipalCouponMap.
    /// </summary>
    public class PrincipalCouponMap : EntityTypeConfiguration<PrincipalCoupon>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PrincipalCouponMap" /> class.
        /// </summary>
        public PrincipalCouponMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PrincipalCoupon");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.AddTime).HasColumnName("AddTime");
            this.Property(t => t.EffectiveStartTime).HasColumnName("EffectiveStartTime");
            this.Property(t => t.EffectiveEndTime).HasColumnName("EffectiveEndTime");
            this.Property(t => t.UseFlag).HasColumnName("UseFlag");
            this.Property(t => t.UseTime).HasColumnName("UseTime");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}