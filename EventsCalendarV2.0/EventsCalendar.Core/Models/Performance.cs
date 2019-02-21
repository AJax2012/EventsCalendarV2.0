using System;
using EventsCalendar.Core.Validation;

namespace EventsCalendar.Core.Models
{
    public class Performance
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int SeatsRemaining { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime EventDateTime { get; set; }
        public Performer Performer { get; set; }
        public int PerformerId { get; set; }
        public Venue Venue { get; set; }
        public int VenueId { get; set; }
    }
}
