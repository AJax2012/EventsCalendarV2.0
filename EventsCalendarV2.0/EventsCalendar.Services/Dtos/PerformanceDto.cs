using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Services.Dtos
{
    public class PerformanceDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Event Date and Time")]
        public DateTime EventDateTime { get; set; }

        public PerformerDto PerformerDto { get; set; }

        public VenueDto VenueDto { get; set; }

        public ICollection<ReservationDto> Reservations { get; set; }
    }
}
