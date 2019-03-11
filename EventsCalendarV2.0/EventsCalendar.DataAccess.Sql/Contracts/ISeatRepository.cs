using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.DataAccess.Sql.Contracts
{
    public interface ISeatRepository : IRepository<Seat>
    {
        void BulkInsertSeats(int numberOfSeats, SeatType type, int venueId);
        void BulkDeleteSeats(int numberOfSeats, SeatType type, int venueId);
        void DeleteAllVenueSeats(int venueId);
    }
}
