// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// File             : OldDBContext.cs
// Created          : 2015-08-02  7:06 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-08-02  7:19 AM
// ***********************************************************************
// <copyright file="OldDBContext.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;

namespace DataTransfer.Models
{
    /// <summary>
    ///     OldDBContext.
    /// </summary>
    public class OldDBContext : DbContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OldDBContext" /> class.
        /// </summary>
        public OldDBContext()
            : base("name=OldDBContext")
        {
        }

        /// <summary>
        ///     Gets or sets the agreements.
        /// </summary>
        /// <value>The agreements.</value>
        public virtual DbSet<Agreements> Agreements { get; set; }

        /// <summary>
        ///     Gets or sets the json jby account transaction.
        /// </summary>
        /// <value>The json jby account transaction.</value>
        public virtual DbSet<JsonJBYAccountTransaction> JsonJBYAccountTransaction { get; set; }

        /// <summary>
        ///     Gets or sets the json jby order.
        /// </summary>
        /// <value>The json jby order.</value>
        public virtual DbSet<JsonJBYOrder> JsonJBYOrder { get; set; }

        /// <summary>
        ///     Gets or sets the json product.
        /// </summary>
        /// <value>The json product.</value>
        public virtual DbSet<JsonProduct> JsonProduct { get; set; }

        /// <summary>
        ///     Gets or sets the json settle account transaction.
        /// </summary>
        /// <value>The json settle account transaction.</value>
        public virtual DbSet<JsonSettleAccountTransaction> JsonSettleAccountTransaction { get; set; }

        /// <summary>
        ///     Gets or sets the json user.
        /// </summary>
        /// <value>The json user.</value>
        public virtual DbSet<JsonUser> JsonUser { get; set; }

        /// <summary>
        ///     Gets or sets the trans bank card.
        /// </summary>
        /// <value>The trans bank card.</value>
        public virtual DbSet<TransBankCard> TransBankCard { get; set; }

        /// <summary>
        ///     Gets or sets the trans jby order information.
        /// </summary>
        /// <value>The trans jby order information.</value>
        public virtual DbSet<TransJbyOrderInfo> TransJbyOrderInfo { get; set; }

        /// <summary>
        ///     Gets or sets the trans order information.
        /// </summary>
        /// <value>The trans order information.</value>
        public virtual DbSet<TransOrderInfo> TransOrderInfo { get; set; }

        /// <summary>
        ///     Gets or sets the state of the trans regular product.
        /// </summary>
        /// <value>The state of the trans regular product.</value>
        public virtual DbSet<TransRegularProductState> TransRegularProductState { get; set; }

        /// <summary>
        ///     Gets or sets the trans settle account transaction.
        /// </summary>
        /// <value>The trans settle account transaction.</value>
        public virtual DbSet<TransSettleAccountTransaction> TransSettleAccountTransaction { get; set; }

        /// <summary>
        ///     Gets or sets the trans user information.
        /// </summary>
        /// <value>The trans user information.</value>
        public virtual DbSet<TransUserInfo> TransUserInfo { get; set; }

        /// <summary>
        ///     Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
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

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.AccountTransactionIdentifier)
                .IsUnicode(false);

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.Args)
                .IsUnicode(false);

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.Cellphone)
                .IsUnicode(false);

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.OrderId)
                .IsUnicode(false);

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.OrderNo)
                .IsUnicode(false);

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.Principal)
                .HasPrecision(18, 6);

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.ProductId)
                .IsUnicode(false);

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<TransJbyOrderInfo>()
                .Property(e => e.Yield)
                .HasPrecision(5, 3);

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