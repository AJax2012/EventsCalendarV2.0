using EventsCalendar.Core.Models;
using System.Collections.Generic;

namespace EventsCalendar.Core.Contracts
{
    public interface IReservationRepository : IGuidRepository<Reservation>
    {
        void BulkInsertReservations(IEnumerable<SimpleReservation> reservations, int performanceId);
        void BulkDeleteReservations(int numberOfReservations, decimal price, int performanceId);
        void DeleteAllPerformanceReservations(int performanceId);
        void ChangeReservationPrices(UpdatePricesObject update);
        ReservationPrices GetPrices(int performanceId);
    }
}
