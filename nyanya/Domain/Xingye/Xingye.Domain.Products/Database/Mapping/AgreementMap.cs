// FileInformation: nyanya/Cqrs.Domain.Product/AgreementMap.cs
// CreatedTime: 2014/07/27   9:04 PM
// LastUpdatedTime: 2014/08/12   12:45 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Products.Models;

namespace Xingye.Domain.Products.Database.Mapping
{
    internal class AgreementMap : EntityTypeConfiguration<Agreement>
    {
        internal AgreementMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Content)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Agreements", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Content).HasColumnName("Content");
        }
    }
}