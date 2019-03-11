using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.Contracts
{
    public interface ITicketViewModel
    {
        TicketDto Ticket { get; set; }
        int PerformanceId { get; set; }
        decimal BudgetPrice { get; set; }
        decimal ModeratePrice { get; set; }
        decimal PremierPrice { get; set; }
        SeatCapacityDto SeatCapacity { get; set; }
    }
}