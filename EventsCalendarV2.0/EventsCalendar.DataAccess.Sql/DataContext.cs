using System.Data.Entity;
using EventsCalendar.Core.Models;

namespace EventsCalendar.DataAccess.Sql
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("EventCalendarDataContext")
        {
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Genre> Genres { get; set; }
        //public DbSet<CustomImage> CustomImages { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<PerformerType> PerformerTypes { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Venue> Venues { get; set; }
    }
}
