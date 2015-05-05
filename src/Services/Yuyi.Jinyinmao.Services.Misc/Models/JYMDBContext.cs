// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-05-04  11:50 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-05-04  11:53 AM
// ***********************************************************************
// <copyright file="JYMDBContext.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using Moe.EntityFramework;
using Yuyi.Jinyinmao.Service.Models.Mapping;

namespace Yuyi.Jinyinmao.Service.Models
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
            modelBuilder.Configurations.Add(new VeriCodeMap());
        }
    }
}