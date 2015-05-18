// FileInformation: nyanya/Crqs.Domain.Meow/FeedbackMap.cs
// CreatedTime: 2014/07/26   7:31 PM
// LastUpdatedTime: 2014/08/09   9:33 AM

using System.Data.Entity.ModelConfiguration;
using Xingye.Domain.Meow.Models;

namespace Xingye.Domain.Meow.Database.Mapping
{
    internal class FeedbackMap : EntityTypeConfiguration<Feedback>
    {
        internal FeedbackMap()
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
            this.ToTable("Feedbacks", "dbo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Cellphone).HasColumnName("Cellphone");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Time).HasColumnName("Time");
        }
    }
}