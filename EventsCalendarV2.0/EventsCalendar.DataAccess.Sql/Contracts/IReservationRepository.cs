using System.Collections.Generic;
using EventsCalendar.Core.Models.Reservations;

namespace EventsCalendar.DataAccess.Sql.Contracts
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
