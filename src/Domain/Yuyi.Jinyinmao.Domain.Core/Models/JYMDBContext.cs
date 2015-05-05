// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  5:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-05  7:03 PM
// ***********************************************************************
// <copyright file="JYMDBContext.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using Moe.EntityFramework;
using Yuyi.Jinyinmao.Domain.Models.Mapping;

namespace Yuyi.Jinyinmao.Domain.Models
{
    /// <summary>
    ///     JYMDBContext.
    /// </summary>
    public class JYMDBContext : DbContextBase
    {
        /// <summary>
        ///     Initializes static members of the <see cref="JYMDBContext" /> class.
        /// </summary>
        static JYMDBContext()
        {
            Database.SetInitializer<JYMDBContext>(null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JYMDBContext" /> class.
        /// </summary>
        public JYMDBContext()
            : base("Name=JYMDBContext")
        {
        }

        /// <summary>
        ///     Gets or sets the account transcations.
        /// </summary>
        /// <value>The account transcations.</value>
        public DbSet<AccountTranscation> AccountTranscations { get; set; }

        /// <summary>
        ///     Gets or sets the bank cards.
        /// </summary>
        /// <value>The bank cards.</value>
        public DbSet<BankCard> BankCards { get; set; }

        /// <summary>
        ///     Gets or sets the jby products.
        /// </summary>
        /// <value>The jby products.</value>
        public DbSet<JBYProduct> JBYProducts { get; set; }

        /// <summary>
        ///     Gets or sets the jby transcations.
        /// </summary>
        /// <value>The jby transcations.</value>
        public DbSet<JBYTranscation> JBYTranscations { get; set; }

        /// <summary>
        ///     Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        ///     Gets or sets the regular products.
        /// </summary>
        /// <value>The regular products.</value>
        public DbSet<RegularProduct> RegularProducts { get; set; }

        /// <summary>
        ///     Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     Gets or sets the veri codes.
        /// </summary>
        /// <value>The veri codes.</value>
        public DbSet<VeriCode> VeriCodes { get; set; }

        /// <summary>
        ///     Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountTranscationMap());
            modelBuilder.Configurations.Add(new BankCardMap());
            modelBuilder.Configurations.Add(new JBYProductMap());
            modelBuilder.Configurations.Add(new JBYTranscationMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new RegularProductMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new VeriCodeMap());
        }
    }
}