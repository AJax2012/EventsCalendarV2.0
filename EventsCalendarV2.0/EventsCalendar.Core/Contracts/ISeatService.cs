using System.Collections.Generic;
using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Contracts
{
    public interface ISeatService
    {
        void ChangeAmountOfSeatsInContext(SeatCapacity capacity, int id);
        SeatCapacity GetSeatCapacities(int venueId);
        IEnumerable<Seat> GetSeatsBySeatType(int venueId, SeatType type);
    }
}