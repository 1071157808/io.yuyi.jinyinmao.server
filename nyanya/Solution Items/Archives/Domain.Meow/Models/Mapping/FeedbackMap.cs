// FileInformation: nyanya/Domain.Meow/FeedbackMap.cs
// CreatedTime: 2014/03/17   5:45 PM
// LastUpdatedTime: 2014/03/17   6:33 PM

using System.Data.Entity.ModelConfiguration;

namespace Domain.Meow.Models.Mapping
{
    public class FeedbackMap : EntityTypeConfiguration<Feedback>
    {
        public FeedbackMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Cellphone)
                .HasMaxLength(20);

            this.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Feedbacks", "Meow");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Time).HasColumnName("Time");
        }
    }
}