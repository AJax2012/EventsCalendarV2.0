using EventsCalendar.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EventsCalendar.EntityConfigurations
{
    public class SeatTypeConfiguration : EntityTypeConfiguration<SeatType>
    {
        public SeatTypeConfiguration()
        {
            Property(s => s.Id)
                .HasDatabaseGeneratedOption(
                    DatabaseGeneratedOption.Identity);

            Property(s => s.SeatTypeLevels)
                .IsRequired();

            Property(s => s.Price)
                .HasPrecision(6, 2)
                .IsOptional();
        }
    }
}
