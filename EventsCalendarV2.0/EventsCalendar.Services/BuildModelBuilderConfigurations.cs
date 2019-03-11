using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsCalendar.EntityConfigurations;

namespace EventsCalendar.Services
{
    public class BuildModelBuilderConfigurations
    {
        public DbModelBuilder Builder(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new PerformanceConfiguration());
            modelBuilder.Configurations.Add(new PerformerConfiguration());
            modelBuilder.Configurations.Add(new SeatConfiguration());
            modelBuilder.Configurations.Add(new ReservationConfiguration());
            modelBuilder.Configurations.Add(new TicketConfiguration());
            modelBuilder.Configurations.Add(new VenueConfiguration());
            return modelBuilder;
        }
    }
}
