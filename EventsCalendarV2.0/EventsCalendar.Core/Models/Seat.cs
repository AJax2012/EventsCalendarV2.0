using System.Collections.Generic;

namespace EventsCalendar.Core.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public SeatType SeatType { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
        public ICollection<Performance> Performances { get; set; }

        public Seat()
        {
            Performances = new List<Performance>();
        }
    }
}
