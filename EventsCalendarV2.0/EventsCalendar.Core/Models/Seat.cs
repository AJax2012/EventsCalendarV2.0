using System.Collections.Generic;

namespace EventsCalendar.Core.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public SeatType SeatType { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

        public Seat()
        {
            Reservations = new List<Reservation>();
            SeatType = new SeatType();
        }
    }
}
