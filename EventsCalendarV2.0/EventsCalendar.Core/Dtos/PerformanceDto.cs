using System;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Core.Validation;

namespace EventsCalendar.Core.Dtos
{
    public class PerformanceDto
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int SeatsRemaining { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Event Date and Time")]
        public DateTime EventDateTime { get; set; }

        public PerformerDto PerformerDto { get; set; }

        public VenueDto VenueDto { get; set; }
    }
}
