using EventsCalendar.Core.Dtos;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.Core.ViewModels
{
    public class TicketViewModel
    {
        public TicketDto Ticket { get; set; }

        public int PerformanceId { get; set; }

        [Display(Name = "Budget Price")]
        public decimal BudgetPrice { get; set; }

        [Display(Name = "Moderate Price")]
        public decimal ModeratePrice { get; set; }

        [Display(Name = "Premier Price")]
        public decimal PremierPrice { get; set; }

        public SeatCapacity SeatCapacity { get; set; }
    }
}
