using System.Collections.Generic;
using EventsCalendar.Core.Models.Reservations;

namespace EventsCalendar.Core.Contracts.Repositories
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
