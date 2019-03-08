using EventsCalendar.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EventsCalendar.EntityConfigurations
{
    public class TicketConfiguration : EntityTypeConfiguration<Ticket>
    {
        public TicketConfiguration()
        {
            HasIndex(t => t.Id)
                .IsUnique();

            Property(t => t.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            HasIndex(t => t.ConfirmationNumber)
                .IsUnique();

            Property(t => t.Recipient)
                .IsRequired();

            Property(t => t.Email)
                .IsRequired();

            HasRequired(s => s.Reservation)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
