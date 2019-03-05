using System.Collections.Generic;
using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Contracts
{
    public interface ISeatService
    {
        void ChangeAmountOfSeatsInContext(int budget, int moderate, int premier, int id);
        SeatCapacity GetSeatCapacities(int venueId);
        IEnumerable<Seat> GetSeatsBySeatType(int venueId, SeatType type);
    }
}