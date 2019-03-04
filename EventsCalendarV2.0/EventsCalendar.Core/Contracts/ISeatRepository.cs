using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Contracts
{
    public interface ISeatRepository : IRepository<Seat>
    {
        void BulkInsertSeats(int numberOfSeats, SeatType type, int venueId);
        void DeleteAllVenueSeats(int venueId);
        void BulkDeleteSeats(int numberOfSeats, SeatType type, int venueId);
    }
}
