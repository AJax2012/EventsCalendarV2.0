namespace EventsCalendar.Core.Models
{
    public class SeatType : BaseEntity
    {
        public SeatTypeLevel SeatTypeLevel { get; set; }
        public decimal? Price { get; set; }
        public int NumberOfSeats { get; set; }

        public SeatType()
        {
            SeatTypeLevel = new SeatTypeLevel();
        }
    }
}