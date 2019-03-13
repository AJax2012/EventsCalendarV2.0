using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.DataAccess.Sql.Contracts
{
    public interface ISeatRepository : IRepository<Seat>
    {
        void BulkInsertSeats(int numberOfSeats, int seatTypeId, int venueId);
        void BulkDeleteSeats(int numberOfSeats, int seatTypeId, int venueId);
        void DeleteAllVenueSeats(int venueId);
    }
}
