﻿using System.Data.Entity;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Core.Models.Tickets;

namespace EventsCalendar.DataAccess.Sql
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("EventCalendarDataContext")
        {
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Performance> Performances { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Venue> Venues { get; set; }
    }
}
