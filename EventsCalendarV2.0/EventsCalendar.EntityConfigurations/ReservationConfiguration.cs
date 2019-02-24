using EventsCalendar.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
