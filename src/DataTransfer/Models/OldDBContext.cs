// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : OldDBContext.cs
// Created          : 2015-07-27  6:28 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-07-27  6:40 PM
// ***********************************************************************
// <copyright file="OldDBContext.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;

namespace DataTransfer.Models
{
    public sealed class OldDBContext : DbContext
    {
        public OldDBContext()
            : base("name=OldDBContext")
        {
        }

        public DbSet<Agreements> Agreements { get; set; }

        public DbSet<JsonProduct> JsonProduct { get; set; }

        public DbSet<JsonUser> JsonUser { get; set; }

        public DbSet<TransBankCard> TransBankCard { get; set; }

        public DbSet<TransOrderInfo> TransOrderInfo { get; set; }

        public DbSet<TransRegularProductInfo> TransRegularProductInfo { get; set; }

        public DbSet<TransRegularProductState> TransRegularProductState { get; set; }

        public DbSet<TransSettleAccountTransaction> TransSettleAccountTransaction { get; set; }

        public DbSet<TransUserInfo> TransUserInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransBankCard>()
                .Property(e => e.BankCardNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransBankCard>()
                .Property(e => e.Cellphone)
                .IsUnicode(false);

            modelBuilder.Entity<TransBankCard>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.AccountTransactionIdentifier)
                .IsUnicode(false);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.Args)
                .IsUnicode(false);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.Cellphone)
                .IsUnicode(false);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.ExtraInterest)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.Interest)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.OrderId)
                .IsUnicode(false);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.Principal)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.ProductId)
                .IsUnicode(false);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<TransOrderInfo>()
                .Property(e => e.Yield)
                .HasPrecision(5, 3);

            modelBuilder.Entity<TransRegularProductInfo>()
                .Property(e => e.EndorseImageLink)
                .IsUnicode(false);

            modelBuilder.Entity<TransRegularProductInfo>()
                .Property(e => e.ProductId)
                .IsUnicode(false);

            modelBuilder.Entity<TransRegularProductInfo>()
                .Property(e => e.ProductNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransRegularProductInfo>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransRegularProductInfo>()
                .Property(e => e.Yield)
                .HasPrecision(5, 3);

            modelBuilder.Entity<TransRegularProductState>()
                .Property(e => e.ProductId)
                .IsUnicode(false);

            modelBuilder.Entity<TransRegularProductState>()
                .Property(e => e.EndorseImageLink)
                .IsUnicode(false);

            modelBuilder.Entity<TransRegularProductState>()
                .Property(e => e.PledgeNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransRegularProductState>()
                .Property(e => e.ProductNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransRegularProductState>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransRegularProductState>()
                .Property(e => e.Yield)
                .HasPrecision(5, 3);

            modelBuilder.Entity<TransSettleAccountTransaction>()
                .Property(e => e.OrderId)
                .IsUnicode(false);

            modelBuilder.Entity<TransSettleAccountTransaction>()
                .Property(e => e.BankCardNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.Cellphone)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.CredentialNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.InviteBy)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.JBYLastInterest)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.JBYTotalInterest)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.JBYTotalPricipal)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.JBYWithdrawalableAmount)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.LoginNames)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.OutletCode)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.EncryptedPassword)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.EncryptedPaymentPassword)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.Salt)
                .IsUnicode(false);

            modelBuilder.Entity<TransUserInfo>()
                .Property(e => e.PaymentSalt)
                .IsUnicode(false);
        }
    }
}