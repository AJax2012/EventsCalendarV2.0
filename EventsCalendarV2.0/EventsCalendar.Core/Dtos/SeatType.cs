using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Dtos
{
    class SeatType
    {
        public SeatTypeLevels SeatTypeLevels { get; set; }
        public decimal? Price { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
