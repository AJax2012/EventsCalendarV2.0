using EventsCalendar.Core.Dtos;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.ViewModels
{
    public class TicketViewModel
    {
        public TicketDto Ticket { get; set; }

        [Display(Name = "Number of Budget Seats")]
        public int NumberOfBudget { get; set; }

        [Display(Name = "Number of Moderate Seats")]
        public int NumberOfModerate { get; set; }

        [Display(Name = "Number of Premier Seats")]
        public int NumberOfPremier { get; set; }
    }
}
