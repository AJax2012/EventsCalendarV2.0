using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Dtos
{
    public class SeatTypeDto
    {
        public SeatTypeLevel SeatTypeLevels { get; set; }
        public decimal? Price { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
