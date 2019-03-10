using EventsCalendar.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EventsCalendar.Core.Models.Reservations;

namespace EventsCalendar.EntityConfigurations
{
    public class ReservationConfiguration : EntityTypeConfiguration<Reservation>
    {
        public ReservationConfiguration()
        {
            HasIndex(r => r.Id)
                .IsUnique();

            Property(r => r.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(r => r.Price)
                .HasPrecision(6, 2)
                .IsRequired();

            Property(r => r.IsTaken)
                .IsRequired();

            HasOptional(r => r.Ticket)
                .WithMany()
                .HasForeignKey(r => r.TicketId)
                .WillCascadeOnDelete(false);
        }
    }
}
