using System.Collections.Generic;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Venue;

namespace EventsCalendar.Services.Dtos.Seat
{
    public class SeatDto
    {
        public int Id { get; set; }
        public SeatTypeDto SeatType { get; set; }
        public VenueDto VenueDto { get; set; }
        public ICollection<ReservationDto> Reservations { get; set; }
    }
}
