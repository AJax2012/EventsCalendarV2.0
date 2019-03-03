using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Contracts
{
    public interface ISeatRepository : IRepository<Seat>
    {
        void BulkInsertSeats(int numberOfSeats, SeatTypeLevel level, int venueId);
        void BulkDeleteVenueSeats(int venueId);
    }
}
