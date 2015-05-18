// FileInformation: nyanya/Domain.GoldCat/GoldCatContext.cs
// CreatedTime: 2014/04/04   9:45 AM
// LastUpdatedTime: 2014/04/04   9:56 AM

using Domain.GoldCat.Models.Mapping;
using System.Data.Entity;

namespace Domain.GoldCat.Models
{
    public class GoldCatContext : DbContext
    {
        static GoldCatContext()
        {
            Database.SetInitializer<GoldCatContext>(null);
        }

        public GoldCatContext()
            : base("Name=GoldCatContext")
        {
        }

        public DbSet<GoldCatUser> GoldCatUsers { get; set; }

        public DbSet<GoldCatUserDetails> GoldCatUserDetailses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GoldCatUserMap());
            modelBuilder.Configurations.Add(new GoldCatUserDetailsMap());
        }
    }
}