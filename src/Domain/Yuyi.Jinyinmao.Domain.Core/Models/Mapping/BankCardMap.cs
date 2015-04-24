// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  11:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  11:33 PM
// ***********************************************************************
// <copyright file="BankCardMap.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Yuyi.Jinyinmao.Domain.Models.Mapping
{
    internal class BankCardMap : EntityTypeConfiguration<BankCard>
    {
        internal BankCardMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BankCardNo)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.BankName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CityName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Info)
                .IsRequired();

            this.Property(t => t.Args)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("BankCards");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.BankCardNo).HasColumnName("BankCardNo");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.CityName).HasColumnName("CityName");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.Verified).HasColumnName("Verified");
            this.Property(t => t.VerifiedTime).HasColumnName("VerifiedTime");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.Args).HasColumnName("Args");
        }
    }
}
