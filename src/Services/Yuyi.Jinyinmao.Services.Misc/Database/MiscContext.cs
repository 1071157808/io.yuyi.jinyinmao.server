// ***********************************************************************
// Project          : io.yuyi.jinyinmao.server
// Author           : Siqi Lu
// Created          : 2015-04-11  10:35 AM
//
// Last Modified By : Siqi Lu
// Last Modified On : 2015-04-21  11:44 PM
// ***********************************************************************
// <copyright file="MiscContext.cs" company="Shanghai Yuyi">
//     Copyright ©  2012-2015 Shanghai Yuyi. All rights reserved.
// </copyright>
// ***********************************************************************

using System.Data.Entity;
using Moe.EntityFramework;
using Yuyi.Jinyinmao.Service.Models;

namespace Yuyi.Jinyinmao.Service
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
            : base("cn.com.jinyinmao.db.front")
        {
        }

        /// <summary>
        ///     Gets or sets the veri codes.
        /// </summary>
        /// <value>The veri codes.</value>
        public DbSet<VeriCode> VeriCodes { get; set; }
    }
}
