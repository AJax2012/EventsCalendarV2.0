using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;
using EventsCalendar.WebUI.Validation;

namespace EventsCalendar.WebUI.ViewModels
{
    public class PerformanceViewModel : IPerformanceViewModel
    {
        public ICollection<PerformerDto> Performers { get; set; }
        public ICollection<VenueDto> Venues { get; set; }
        public PerformanceDto Performance { get; set; }
        public SeatCapacityDto SeatsRemaining { get; set; }

        [Display(Name = "Budget Price")]
        public decimal BudgetPrice { get; set; }

        [Display(Name = "Moderate Price")]
        public decimal ModeratePrice { get; set; }

        [Display(Name = "Premier Price")]
        public decimal PremierPrice { get; set; }

        [AfterToday]
        [Display(Name = "Event Date")]
        public string EventDate { get; set; }

        [Display(Name = "Event Time")]
        public string EventTime { get; set; }
    }
}
