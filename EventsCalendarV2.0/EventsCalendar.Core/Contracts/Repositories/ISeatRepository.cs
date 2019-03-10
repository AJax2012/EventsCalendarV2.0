using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.Core.Contracts.Repositories
{
    public interface ISeatRepository : IRepository<Seat>
    {
        void BulkInsertSeats(int numberOfSeats, SeatType type, int venueId);
        void BulkDeleteSeats(int numberOfSeats, SeatType type, int venueId);
        void DeleteAllVenueSeats(int venueId);
    }
}
