using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.WebUI.ViewModels
{
    public class VenueViewModel : IVenueViewModel
    {
        public VenueDto Venue { get; set; }
        public SeatCapacityDto SeatCapacity { get; set; }
    }
}
