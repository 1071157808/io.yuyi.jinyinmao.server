// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-06  3:07 PM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-06  6:58 PM
// ***********************************************************************
// <copyright file="MiscContext.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using Moe.EntityFramework;
using Yuyi.Jinyinmao.Services.Models;

namespace Yuyi.Jinyinmao.Services
{
    /// <summary>
    ///     Class MiscContext.
    /// </summary>
    public class MiscContext : DbContextBase
    {
        static MiscContext()
        {
            IDatabaseInitializer<MiscContext> strategy = new NullDatabaseInitializer<MiscContext>();
            Database.SetInitializer(strategy);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbContextBase" /> class.
        /// </summary>
        public MiscContext()
            : base("jinyinmao-db-server-query")
        {
        }

        /// <summary>
        ///     Gets or sets the veri codes.
        /// </summary>
        /// <value>The veri codes.</value>
        public DbSet<VeriCode> VeriCodes { get; set; }
    }
}
