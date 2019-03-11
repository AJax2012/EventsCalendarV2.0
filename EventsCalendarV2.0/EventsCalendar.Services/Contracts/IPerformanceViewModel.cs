using System.Collections.Generic;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.Contracts
{
    public interface IPerformanceViewModel
    {
        ICollection<PerformerDto> Performers { get; set; }
        ICollection<VenueDto> Venues { get; set; }
        PerformanceDto Performance { get; set; }
        SeatCapacityDto SeatsRemaining { get; set; }
        decimal BudgetPrice { get; set; }
        decimal ModeratePrice { get; set; }
        decimal PremierPrice { get; set; }
        string EventDate { get; set; }
        string EventTime { get; set; }
    }
}