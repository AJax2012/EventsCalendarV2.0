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

            Property(p => p.PerformerType)
                .IsRequired();

            Property(p => p.Genre)
                .IsOptional();

            Property(p => p.Topic)
                .IsOptional();
        }
    }
}
