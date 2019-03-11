using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.Contracts
{
    public interface IVenueViewModel
    {
        VenueDto Venue { get; set; }
        SeatCapacityDto SeatCapacity { get; set; }
    }
}
