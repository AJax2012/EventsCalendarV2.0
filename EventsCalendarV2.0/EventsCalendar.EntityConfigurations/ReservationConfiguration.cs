using EventsCalendar.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EventsCalendar.EntityConfigurations
{
    class ReservationConfiguration : EntityTypeConfiguration<Reservation>
    {
        public ReservationConfiguration()
        {
            HasIndex(s => s.Id)
                .IsUnique();

            Property(s => s.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(s => s.Price)
                .HasPrecision(6, 2)
                .IsRequired();

            Property(s => s.IsTaken)
                .IsRequired();
        }
    }
}
