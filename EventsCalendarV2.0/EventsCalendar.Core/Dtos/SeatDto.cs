using EventsCalendar.Core.Models;
using System.Collections.Generic;

namespace EventsCalendar.Core.Dtos
{
    public class SeatDto
    {
        public int Id { get; set; }
        //public int SeatTypeId { get; set; }
        public SeatType SeatType { get; set; }
        public int VenueId { get; set; }
        public VenueDto VenueDto { get; set; }
        public ICollection<ReservationDto> ReservationDtos { get; set; }
    }
}
