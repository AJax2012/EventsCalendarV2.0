using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Dtos.Seat;
using EventsCalendar.Services.Dtos.Venue;
using EventsCalendar.Services.Validation;

namespace EventsCalendar.WebUI.ViewModels
{
    public class PerformanceViewModel
    {
        public IEnumerable<PerformerDto> Performers { get; set; }
        public IEnumerable<VenueDto> Venues { get; set; }
        public PerformanceDto Performance { get; set; }
        public SeatCapacityDto SeatsRemaining { get; set; }

        [AfterToday(ErrorMessage = "Please select a date tomorrow or later.")]
        [Display(Name = "Date")]
        public string EventDate { get; set; }

        [Display(Name = "Time")]
        public string EventTime { get; set; }
    }
}
