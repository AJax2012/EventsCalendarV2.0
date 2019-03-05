using System.Data.Entity;
using EventsCalendar.EntityConfigurations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventsCalendar.WebUI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("EventCalendarDataContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new PerformanceConfiguration());
            modelBuilder.Configurations.Add(new PerformerConfiguration());
            modelBuilder.Configurations.Add(new SeatConfiguration());
            modelBuilder.Configurations.Add(new ReservationConfiguration());
            modelBuilder.Configurations.Add(new VenueConfiguration());
        }
    }
}