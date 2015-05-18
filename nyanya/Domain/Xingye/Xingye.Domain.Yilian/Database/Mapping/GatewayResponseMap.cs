// FileInformation: nyanya/Xingye.Domain.Yilian/GatewayResponseMap.cs
// CreatedTime: 2014/07/27   6:20 PM
// LastUpdatedTime: 2014/07/27   6:22 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Yilian.Models;

namespace Xingye.Domain.Yilian.Database.Mapping
{
    internal class GatewayResponseMap : EntityTypeConfiguration<GatewayResponse>
    {
        internal GatewayResponseMap()
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
            this.ToTable("GatewayResponse", "dbo");
            this.Property(t => t.RequestIdentifier).HasColumnName("RequestIdentifier");
            this.Property(t => t.ResponseString).HasColumnName("ResponseString");
            this.Property(t => t.SendingTime).HasColumnName("SendingTime");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Result).HasColumnName("Result");
        }
    }
}