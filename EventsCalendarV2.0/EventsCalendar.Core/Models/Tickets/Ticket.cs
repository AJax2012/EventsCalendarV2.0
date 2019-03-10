using System;
using System.Collections.Generic;
using EventsCalendar.Core.Models.Reservations;

namespace EventsCalendar.Core.Models.Tickets
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string ConfirmationNumber { get; set; }
        public string Recipient { get; set; }
        public string Email { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

        public Ticket()
        {
            Reservations = new List<Reservation>();
        }
    }
}
