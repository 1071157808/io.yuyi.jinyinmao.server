// FileInformation: nyanya/Domian.Passport/PassportContext.cs
// CreatedTime: 2014/03/31   3:59 PM
// LastUpdatedTime: 2014/05/04   9:54 AM

using System.Data.Entity;
using Domain.Passport.Models.Mapping;
using Domian.Passport.Models;
using Domian.Passport.Models.Mapping;

namespace Domain.Passport.Models
{
    public class PassportContext : DbContext
    {
        public DbSet<OrderChange> OrderChanges { get; set; }

        /// <summary>
        ///     Gets or sets the user infos. From view, readonly.
        /// </summary>
        /// <value>
        ///     The user infos.
        /// </value>
        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Verification> Verifications { get; set; }

        static PassportContext()
        {
            Database.SetInitializer<PassportContext>(null);
        }

        public PassportContext()
            : base("Name=PassportContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new VerificationMap());
            modelBuilder.Configurations.Add(new UserInfoMap());
            modelBuilder.Configurations.Add(new OrderChangeMap());
        }
    }
}