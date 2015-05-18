// FileInformation: nyanya/Domian.Passport/verificationMap.cs
// CreatedTime: 2014/03/31   3:59 PM
// LastUpdatedTime: 2014/04/03   11:49 AM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Passport.Models.Mapping
{
    public class VerificationMap : EntityTypeConfiguration<Verification>
    {
        public VerificationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Cellphone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Guid)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(60);

            // Table & Column Mappings
            this.ToTable("net_Verifications", "passport_production");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.ErrorCount).HasColumnName("ErrorCount");
            this.Property(t => t.Guid).HasColumnName("Guid");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Times).HasColumnName("Times");
            this.Property(t => t.BuildAt).HasColumnName("BuildAt");
            this.Property(t => t.Verified).HasColumnName("Verified");
            this.Property(t => t.Used).HasColumnName("Used");
        }
    }
}