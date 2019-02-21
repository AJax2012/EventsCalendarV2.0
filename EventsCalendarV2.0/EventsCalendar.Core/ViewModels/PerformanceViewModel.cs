using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Validation;

namespace EventsCalendar.Core.ViewModels
{
    public class PerformanceViewModel
    {
        public IEnumerable<PerformerDto> Performers { get; set; }
        public IEnumerable<VenueDto> Venues { get; set; }
        public PerformanceDto Performance { get; set; }
        public string GoogleMapsSrcUrl { get; set; }

        [AfterToday]
        [Display(Name = "Event Date")]
        public string EventDate { get; set; }

        [Display(Name = "Event Time")]
        public string EventTime { get; set; }
    }
}
