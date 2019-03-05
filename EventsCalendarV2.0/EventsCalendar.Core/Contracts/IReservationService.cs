using System.Collections.Generic;
using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Contracts
{
    public interface IReservationService
    {
        IEnumerable<SimpleReservation> CombineReservations(IEnumerable<SimpleReservation> budget, IEnumerable<SimpleReservation> moderate, IEnumerable<SimpleReservation> premier);
        IEnumerable<SimpleReservation> GetSimpleReservations(int venueId, SeatType type, decimal price);
        void SetNewReservationPrices(int performanceId, ReservationPrices prices);
    }
}