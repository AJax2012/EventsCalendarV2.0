using System;

namespace EventsCalendar.Core.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Recipient { get; set; }
        public string Email { get; set; }
        public Reservation Reservation { get; set; }
    }
}
