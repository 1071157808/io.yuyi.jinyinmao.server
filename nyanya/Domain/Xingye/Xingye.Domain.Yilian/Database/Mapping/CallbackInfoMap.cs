// FileInformation: nyanya/Xingye.Domain.Yilian/CallbackInfoMap.cs
// CreatedTime: 2014/07/27   6:13 PM
// LastUpdatedTime: 2014/07/27   6:19 PM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Yilian.Models;

namespace Xingye.Domain.Yilian.Database.Mapping
{
    internal class CallbackInfoMap : EntityTypeConfiguration<CallbackInfo>
    {
        internal CallbackInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestIdentifier);

            // Properties
            this.Property(t => t.RequestIdentifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CallbackString)
                .IsRequired();

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("CallbackInfo", "dbo");
            this.Property(t => t.RequestIdentifier).HasColumnName("RequestIdentifier");
            this.Property(t => t.CallbackString).HasColumnName("CallbackString");
            this.Property(t => t.CallbackTime).HasColumnName("CallbackTime");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Result).HasColumnName("Result");
        }
    }
}