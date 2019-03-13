using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.EntityConfigurations
{
    public class SeatTypeConfiguration : EntityTypeConfiguration<SeatType>
    {
        public SeatTypeConfiguration()
        {
            Property(p => p.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(p => p.Name)
                .IsRequired();
        }
    }
}
