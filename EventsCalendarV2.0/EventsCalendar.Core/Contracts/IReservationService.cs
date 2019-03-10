using System.Collections.Generic;
using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Contracts
{
    public interface IReservationService
    {
        IEnumerable<SimpleReservation> CombineSimpleReservations(SimpleReservationsByType reservations);
        IEnumerable<SimpleReservation> CreateSimpleReservations(int venueId, SeatType type, decimal price);
        SeatCapacity GetSeatsRemaining(int performanceId);
        IEnumerable<Reservation> GetReservations(SeatCapacity capacity, int performanceId);
        void SetNewReservationPrices(int performanceId, ReservationPrices prices);
    }
}