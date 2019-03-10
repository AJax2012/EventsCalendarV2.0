using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EventsCalendar.Services.SeatServices
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        /**
         * Edits the number of seats in the database
         * Checks if new number is more or less than the original
         * removes or adds Seats as needed
         */
        public void ChangeAmountOfSeatsInContext(SeatCapacity capacity, int id)
        {

            if (capacity.Budget > 0)
                _seatRepository.BulkInsertSeats(capacity.Budget, SeatType.Budget, id);
            else if (capacity.Budget < 0)
                _seatRepository.BulkDeleteSeats(capacity.Budget, SeatType.Budget, id);

            if (capacity.Moderate > 0)
                _seatRepository.BulkInsertSeats(capacity.Moderate, SeatType.Moderate, id);
            else if (capacity.Moderate < 0)
                _seatRepository.BulkDeleteSeats(capacity.Moderate, SeatType.Moderate, id);

            if (capacity.Premier > 0)
                _seatRepository.BulkInsertSeats(capacity.Premier, SeatType.Premier, id);
            else if (capacity.Premier < 0)
                _seatRepository.BulkDeleteSeats(capacity.Premier, SeatType.Premier, id);
        }

        /**
         * Returns a SeatCapacity object, separated into
         * a count of each SeatType
         */
        public SeatCapacity GetSeatCapacities(int venueId)
        {
            var capacity = new SeatCapacity();
            var allSeats = _seatRepository
                .Collection()
                .Where(seat => seat.VenueId == venueId)
                .ToList();

            capacity.Budget = allSeats
                .Where(seat => seat.SeatType.Equals(SeatType.Budget))
                .Count();

            capacity.Moderate = allSeats
                .Where(seat => seat.SeatType == SeatType.Moderate)
                .Count();

            capacity.Premier = allSeats
                .Where(seat => seat.SeatType == SeatType.Premier)
                .Count();

            capacity.Total = allSeats.Count();

            return capacity;
        }

        public IEnumerable<Seat> GetSeatsBySeatType(int venueId, SeatType type)
        {
            return _seatRepository.Collection()
                .Where(seat => seat.VenueId == venueId)
                .Where(seat => seat.SeatType == type)
                .ToList();
        }

        /**
         * loops through amounts in seats array. sets essential values for each seat
         */
        private void CreateSeatsForNewVenue(int capacity, SeatType type, Venue venue)
        {
            for (var i = 0; i >= capacity; i++)
            {
                var seat = new Seat();
                seat.SeatType = type;
                venue.Seats.Add(seat);
            };
        }
    }
}
