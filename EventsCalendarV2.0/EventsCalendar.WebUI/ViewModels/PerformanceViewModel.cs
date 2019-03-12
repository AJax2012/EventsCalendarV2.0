using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Validation;

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

        [AfterToday(ErrorMessage = "Please choose Date/Time after today.")]
        [Display(Name = "Date")]
        public string EventDate { get; set; }

        [Display(Name = "Time")]
        public string EventTime { get; set; }
    }
}
