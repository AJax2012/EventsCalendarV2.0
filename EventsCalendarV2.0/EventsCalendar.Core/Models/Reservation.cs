using System;

namespace EventsCalendar.Core.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public int PerformanceId { get; set; }
        public Performance Performance { get; set; }
        public int? TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public bool IsTaken { get; set; }
    }
}
