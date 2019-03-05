using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Contracts
{
    public interface IReservationRepository : IGuidRepository<Reservation>
    {
        void BulkInsertReservations(int numberOfReservations, SeatType type, int performanceId);
        void DeleteAllPerformanceReservations(int performanceId);
        void BulkDeleteReservations(int numberOfReservations, SeatType type, int performanceId);
    }
}
