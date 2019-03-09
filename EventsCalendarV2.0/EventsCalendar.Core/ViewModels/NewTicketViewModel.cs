using EventsCalendar.Core.Dtos;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.ViewModels
{
    public class NewTicketViewModel
    {
        public TicketViewModel Ticket { get; set; }

        [Display(Name = "Budget Seat Price")]
        public decimal PriceOfBudget { get; set; }

        [Display(Name = "Moderate Seat Price")]
        public decimal PriceOfModerate { get; set; }

        [Display(Name = "Premier Seats Price")]
        public decimal PriceOfPremier { get; set; }
    }
}
