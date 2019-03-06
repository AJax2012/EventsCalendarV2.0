using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EventsCalendar.Core.Models;

namespace EventsCalendar.EntityConfigurations
{
    public class PerformanceConfiguration : EntityTypeConfiguration<Performance>
    {
        public PerformanceConfiguration()
        {
            Property(p => p.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(p => p.IsActive)
                .IsRequired();

            Property(p => p.EventDateTime)
                .IsRequired();

            Property(p => p.BudgetSeatsRemaining)
                .IsRequired();

            Property(p => p.ModerateSeatsRemaining)
                .IsRequired();

            Property(p => p.PremierSeatsRemaining)
                .IsRequired();

            HasRequired(p => p.Venue)
                .WithMany(v => v.Performances)
                .HasForeignKey(p => p.VenueId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Performer)
                .WithMany(p => p.Performances)
                .HasForeignKey(p => p.PerformerId)
                .WillCascadeOnDelete(false);

            HasMany(p => p.Reservations)
                .WithRequired(r => r.Performance)
                .HasForeignKey(r => r.PerformanceId)
                .WillCascadeOnDelete(false);
        }
    }
}
