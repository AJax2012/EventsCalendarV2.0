using EventsCalendar.Core.Models;
using System.Collections.Generic;

namespace EventsCalendar.Core.Contracts
{
    public interface IReservationRepository : IGuidRepository<Reservation>
    {
        void BulkInsertReservations(IEnumerable<SimpleReservation> reservations, int performanceId);
        void DeleteAllPerformanceReservations(int performanceId);
        void BulkDeleteReservations(int numberOfReservations, SeatType type, int performanceId);
    }
}
