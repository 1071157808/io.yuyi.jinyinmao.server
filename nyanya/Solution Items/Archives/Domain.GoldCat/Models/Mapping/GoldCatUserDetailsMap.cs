using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Domain.GoldCat.Models.Mapping
{
    public class GoldCatUserDetailsMap : EntityTypeConfiguration<GoldCatUserDetails>
    {
        public GoldCatUserDetailsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Question)
                .HasMaxLength(50);

            this.Property(t => t.Answer)
                .HasMaxLength(10);

            this.Property(t => t.LastSignInIp)
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("user_details", "gold_cat_production");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.UserId).HasColumnName("user_id");
            this.Property(t => t.ErrorAnswerCount).HasColumnName("error_answer_count");
            this.Property(t => t.ErrorSignInCount).HasColumnName("error_sign_in_count");
            this.Property(t => t.LastSignInAt).HasColumnName("last_sign_in_at");
            this.Property(t => t.Question).HasColumnName("question");
            this.Property(t => t.Answer).HasColumnName("answer");
            this.Property(t => t.LastSignInIp).HasColumnName("last_sign_in_ip");
            this.Property(t => t.UpdatedAt).HasColumnName("updated_at");
            this.Property(t => t.LastFailSignInAt).HasColumnName("last_fail_sign_in_at");
        }
    }
}
