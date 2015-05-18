// FileInformation: nyanya/Domian.Passport/UserMap.cs
// CreatedTime: 2014/03/31   3:59 PM
// LastUpdatedTime: 2014/04/01   12:51 AM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Passport.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Cellphone)
                .HasMaxLength(15);

            this.Property(t => t.Email)
                .HasMaxLength(80);

            this.Property(t => t.EncryptedPassword)
                .HasMaxLength(80);

            this.Property(t => t.Name)
                .HasMaxLength(255);

            this.Property(t => t.Salt)
                .HasMaxLength(80);

            this.Property(t => t.Uuid)
                .HasMaxLength(80);

            this.Property(t => t.UnspayToken)
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("users", "passport_production");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.AppId).HasColumnName("app_id");
            this.Property(t => t.Cellphone).HasColumnName("cellphone");
            this.Property(t => t.Email).HasColumnName("email");
            this.Property(t => t.EncryptedPassword).HasColumnName("encrypted_password");
            this.Property(t => t.Name).HasColumnName("name");
            this.Property(t => t.Salt).HasColumnName("salt");
            this.Property(t => t.Uuid).HasColumnName("uuid");
            this.Property(t => t.CreatedAt).HasColumnName("created_at");
            this.Property(t => t.UpdatedAt).HasColumnName("updated_at");
            this.Property(t => t.ErrorSignInCount).HasColumnName("error_sign_in_count");
            this.Property(t => t.LastFailSignInAt).HasColumnName("last_fail_sign_in_at");
            this.Property(t => t.LastSuccessfulSignInAt).HasColumnName("last_successful_sign_in_at");
            this.Property(t => t.UnspayToken).HasColumnName("unspay_token");
            this.Property(t => t.InvitationCodeId).HasColumnName("invitation_code_id");
            this.Property(t => t.HandlerId).HasColumnName("handler_id");
            this.Property(t => t.MLogonFailedCount).HasColumnName("MLogonFailedCount");
        }
    }
}