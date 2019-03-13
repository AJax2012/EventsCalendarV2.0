using System.ComponentModel.DataAnnotations;
using EventsCalendar.Services.Dtos;
using EventsCalendar.Services.Dtos.Seat;

namespace EventsCalendar.WebUI.ViewModels
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

        public SeatCapacityDto SeatCapacity { get; set; }
    }
}
