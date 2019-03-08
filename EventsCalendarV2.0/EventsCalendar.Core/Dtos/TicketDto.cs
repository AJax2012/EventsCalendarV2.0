using System;

namespace EventsCalendar.Core.Dtos
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public string ConfirmationNumber { get; set; }
        public string Recipient { get; set; }
        public string Email { get; set; }
        public ReservationDto Reservation { get; set; }
    }
}
