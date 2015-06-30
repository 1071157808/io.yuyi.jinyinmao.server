// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-29  5:29 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-28  11:49 AM
// ***********************************************************************
// <copyright file="JYMDBContext.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure;
using Moe.EntityFramework;
using Yuyi.Jinyinmao.Domain.Models.Mapping;

namespace Yuyi.Jinyinmao.Domain.Models
{
    /// <summary>
    ///     JYMDBContext.
    /// </summary>
    public sealed class JYMDBContext : DbContextBase
    {
        /// <summary>
        ///     The connectiong string
        /// </summary>
        private static readonly string ConnectionString;

        /// <summary>
        ///     Initializes static members of the <see cref="JYMDBContext" /> class.
        /// </summary>
        static JYMDBContext()
        {
            Database.SetInitializer<JYMDBContext>(null);
            ConnectionString = CloudConfigurationManager.GetSetting("JYMDBContextConnectionString");
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JYMDBContext" /> class.
        /// </summary>
        public JYMDBContext()
            : base(ConnectionString)
        {
        }

        /// <summary>
        ///     Gets or sets the account transactions.
        /// </summary>
        /// <value>The account transactions.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public DbSet<AccountTransaction> AccountTransactions { get; set; }

        /// <summary>
        ///     Gets or sets the bank cards.
        /// </summary>
        /// <value>The bank cards.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public DbSet<BankCard> BankCards { get; set; }

        /// <summary>
        ///     Gets or sets the jby products.
        /// </summary>
        /// <value>The jby products.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public DbSet<JBYProduct> JBYProducts { get; set; }

        /// <summary>
        ///     Gets or sets the jby transactions.
        /// </summary>
        /// <value>The jby transactions.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public DbSet<JBYTransaction> JBYTransactions { get; set; }

        /// <summary>
        ///     Gets or sets the orders.
        /// </summary>
        /// <value>The orders.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        ///     Gets or sets the regular products.
        /// </summary>
        /// <value>The regular products.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public DbSet<RegularProduct> RegularProducts { get; set; }

        /// <summary>
        ///     Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
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
            modelBuilder.Configurations.Add(new AccountTransactionMap());
            modelBuilder.Configurations.Add(new BankCardMap());
            modelBuilder.Configurations.Add(new JBYProductMap());
            modelBuilder.Configurations.Add(new JBYTransactionMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new RegularProductMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new VeriCodeMap());
        }
    }
}