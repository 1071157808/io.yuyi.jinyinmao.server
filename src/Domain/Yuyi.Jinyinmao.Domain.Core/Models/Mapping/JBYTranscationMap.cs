// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  5:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-27  1:27 PM
// ***********************************************************************
// <copyright file="JBYTransactionMap.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity.ModelConfiguration;

namespace Yuyi.Jinyinmao.Domain.Models.Mapping
{
    /// <summary>
    ///     JBYTransactionMap.
    /// </summary>
    public class JBYTransactionMap : EntityTypeConfiguration<JBYTransaction>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JBYTransactionMap" /> class.
        /// </summary>
        public JBYTransactionMap()
        {
            // Primary Key
            this.HasKey(t => t.TransactionIdentifier);

            // Properties
            this.Property(t => t.TransactionIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AccountTransactionIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.JBYProductIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TransDesc)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.UserInfo)
                .IsRequired();

            this.Property(t => t.Info)
                .IsRequired();

            this.Property(t => t.Args)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("JBYTransactions");
            this.Property(t => t.TransactionIdentifier).HasColumnName("TransactionIdentifier");
            this.Property(t => t.AccountTransactionIdentifier).HasColumnName("AccountTransactionIdentifier");
            this.Property(t => t.JBYProductIdentifier).HasColumnName("JBYProductIdentifier");
            this.Property(t => t.UserIdentifier).HasColumnName("UserIdentifier");
            this.Property(t => t.TradeCode).HasColumnName("TradeCode");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.TransactionTime).HasColumnName("TransactionTime");
            this.Property(t => t.ResultCode).HasColumnName("ResultCode");
            this.Property(t => t.ResultTime).HasColumnName("ResultTime");
            this.Property(t => t.TransDesc).HasColumnName("TransDesc");
            this.Property(t => t.UserInfo).HasColumnName("UserInfo");
            this.Property(t => t.Info).HasColumnName("Info");
            this.Property(t => t.Args).HasColumnName("Args");
        }
    }
}