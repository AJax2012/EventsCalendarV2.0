using System;
using System.Collections.Generic;

namespace EventsCalendar.Core.Models
{
    public class Performance
    {
        public int Id { get; set; }
        public int BudgetSeatsRemaining { get; set; }
        public int ModerateSeatsRemaining { get; set; }
        public int PremierSeatsRemaining { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime EventDateTime { get; set; }
        public Performer Performer { get; set; }
        public int PerformerId { get; set; }
        public Venue Venue { get; set; }
        public int VenueId { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

        public Performance()
        {
            Reservations = new List<Reservation>();
        }
    }
}
