using System.Collections.Generic;
using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.Services.Contracts.Services
{
    public interface ISeatService
    {
        void ChangeAmountOfSeatsInContext(SeatCapacity capacity, int id);
        SeatCapacityDto GetSeatCapacities(int venueId);
        IEnumerable<Seat> GetSeatsBySeatType(int venueId, SeatType type);
    }
}