using System.ComponentModel.DataAnnotations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.WebUI.ViewModels
{
    public class TicketViewModel : ITicketViewModel
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
