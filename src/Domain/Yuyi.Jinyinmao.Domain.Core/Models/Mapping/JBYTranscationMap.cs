// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-24  11:18 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-24  11:34 PM
// ***********************************************************************
// <copyright file="JBYTranscationMap.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Yuyi.Jinyinmao.Domain.Models.Mapping
{
    internal class JBYTranscationMap : EntityTypeConfiguration<JBYTranscation>
    {
        internal JBYTranscationMap()
        {
            // Primary Key
            this.HasKey(t => t.TranscationIdentifier);

            // Properties
            this.Property(t => t.TranscationIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Amount)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.TransDesc)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.AgreementsInfo)
                .IsRequired();

            this.Property(t => t.UserInfo)
                .IsRequired();

            this.Property(t => t.Info)
                .IsRequired();

            this.Property(t => t.Args)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("JBYTranscations");
            this.Property(t => t.TranscationIdentifier).HasColumnName("TranscationIdentifier");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.TradeCode).HasColumnName("TradeCode");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.TranscationTime).HasColumnName("TranscationTime");
            this.Property(t => t.ResultCode).HasColumnName("ResultCode");
            this.Property(t => t.ResultTime).HasColumnName("ResultTime");
            this.Property(t => t.TransDesc).HasColumnName("TransDesc");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.AgreementsInfo).HasColumnName("AgreementsInfo");
            this.Property(t => t.UserInfo).HasColumnName("UserInfo");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.Args).HasColumnName("Args");
        }
    }
}
