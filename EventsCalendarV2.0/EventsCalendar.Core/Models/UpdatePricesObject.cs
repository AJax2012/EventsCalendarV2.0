namespace EventsCalendar.Core.Models
{
    public class UpdatePricesObject
    {
        public decimal Price { get; set; }
        public SeatType Type { get; set; }
        public int PerformanceId { get; set; }
    }
}
