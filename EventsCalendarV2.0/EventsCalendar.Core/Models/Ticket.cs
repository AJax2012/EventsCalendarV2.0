using System;
using System.Collections.Generic;

namespace EventsCalendar.Core.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string ConfirmationNumber { get; set; }
        public string Recipient { get; set; }
        public string Email { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }

        public Ticket()
        {
            Reservations = new List<Reservation>();
        }
    }
}
