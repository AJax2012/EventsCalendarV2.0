using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EventsCalendar.Core.Models;

namespace EventsCalendar.EntityConfigurations
{
    public class PerformerConfiguration : EntityTypeConfiguration<Performer>
    {
        public PerformerConfiguration()
        {
            HasKey(p => p.Id);

            HasIndex(p => p.Name)
                .IsUnique();

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(p => p.IsActive)
                .IsRequired();

            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            //HasRequired(p => p.CustomImage)
            //    .WithMany()
            //    .HasForeignKey(p => p.CustomImageId)
            //    .WillCascadeOnDelete(true);

            HasRequired(p => p.PerformerType)
                .WithMany(p => p.Performers)
                .HasForeignKey(p => p.PerformerTypeId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.Genre)
                .WithMany()
                .HasForeignKey(p => p.GenreId)
                .WillCascadeOnDelete(false);

            HasOptional(p => p.Topic)
                .WithMany()
                .HasForeignKey(p => p.TopicId)
                .WillCascadeOnDelete(false);
        }
    }
}
