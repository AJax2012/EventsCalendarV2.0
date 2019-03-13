using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.Core.Models.Reservations
{
    public class UpdatePricesObject
    {
        public decimal Price { get; set; }
        public int SeatTypeId { get; set; }
        public SeatType Type { get; set; }
        public int PerformanceId { get; set; }
    }
}
