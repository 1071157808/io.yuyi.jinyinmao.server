// FileInformation: nyanya/Domain.Amp/AmpContext.cs
// CreatedTime: 2014/03/30   8:08 PM
// LastUpdatedTime: 2014/04/23   12:23 AM

using System.Data.Entity;
using Domain.Amp.Models.Mapping;

namespace Domain.Amp.Models
{
    /// <summary>
    ///     AmpContext
    /// </summary>
    public class AmpContext : DbContext
    {
        static AmpContext()
        {
            Database.SetInitializer<AmpContext>(null);
        }

        public AmpContext()
            : base("Name=AmpContext")
        {
        }

        public DbSet<MeowConfiguration> MeowConfigurations { get; set; }

        /// <summary>
        ///     Gets or sets the products. From view, readonly.
        /// </summary>
        /// <value>
        ///     The products.
        /// </value>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        ///     Gets or sets the top products. From view, readonly.
        /// </summary>
        /// <value>
        ///     The top products.
        /// </value>
        public DbSet<TopProduct> TopProducts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MeowConfigurationMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new TopProductMap());
        }
    }
}