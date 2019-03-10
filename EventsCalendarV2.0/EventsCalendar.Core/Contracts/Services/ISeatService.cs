using System.Collections.Generic;
using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.Core.Contracts.Services
{
    public interface ISeatService
    {
        void ChangeAmountOfSeatsInContext(SeatCapacity capacity, int id);
        SeatCapacity GetSeatCapacities(int venueId);
        IEnumerable<Seat> GetSeatsBySeatType(int venueId, SeatType type);
    }
}