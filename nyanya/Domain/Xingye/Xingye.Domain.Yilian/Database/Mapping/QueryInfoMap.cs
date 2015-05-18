// FileInformation: nyanya/Xingye.Domain.Yilian/QueryInfoMap.cs
// CreatedTime: 2014/07/27   6:23 PM
// LastUpdatedTime: 2014/07/27   6:27 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Yilian.Models;

namespace Xingye.Domain.Yilian.Database.Mapping
{
    internal class QueryInfoMap : EntityTypeConfiguration<QueryInfo>
    {
        internal QueryInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestIdentifier);

            // Properties
            this.Property(t => t.RequestIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ResponseString)
                .IsRequired();

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("QueryInfo", "dbo");
            this.Property(t => t.RequestIdentifier).HasColumnName("RequestIdentifier");
            this.Property(t => t.ResponseString).HasColumnName("ResponseString");
            this.Property(t => t.QueryTime).HasColumnName("QueryTime");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Result).HasColumnName("Result");
        }
    }
}