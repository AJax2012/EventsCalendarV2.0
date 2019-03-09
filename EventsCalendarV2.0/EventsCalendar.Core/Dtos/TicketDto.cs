using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Dtos
{
    public class TicketDto
    {
        public Guid Id { get; set; }

        public string Recipient { get; set; }

        public string Email { get; set; }

        [Display(Name = "Total")]
        public decimal TotalPrice { get; set; }

        [Display(Name ="Confirmation Number")]
        public string ConfirmationNumber { get; set; }

        public IEnumerable<ReservationDto> Reservations { get; set; }
    }
}
