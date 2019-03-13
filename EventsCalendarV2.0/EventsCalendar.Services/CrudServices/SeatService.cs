using System.Collections.Generic;
using System.Linq;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos.Seat;

namespace EventsCalendar.Services.CrudServices
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public SeatCapacityDto CalculateAmountOfSeatsToChange(SeatCapacityDto oldSeats, SeatCapacityDto newSeats)
        {
            return new SeatCapacityDto
            {
                Budget = newSeats.Budget - oldSeats.Budget,
                Moderate = newSeats.Moderate - oldSeats.Moderate,
                Premier = newSeats.Premier - oldSeats.Premier
            };
        }

        /**
         * Edits the number of seats in the database
         * Checks if new number is more or less than the original
         * removes or adds Seats as needed
         */
        public void ChangeAmountOfSeatsInContext(SeatCapacityDto capacity, int id)
        {
            if (capacity.Budget > 0)
                _seatRepository.BulkInsertSeats(capacity.Budget, (int) SeatTypeDto.Budget, id);
            else if (capacity.Budget < 0)
                _seatRepository.BulkDeleteSeats(capacity.Budget, (int) SeatTypeDto.Budget, id);

            if (capacity.Moderate > 0)
                _seatRepository.BulkInsertSeats(capacity.Moderate, (int) SeatTypeDto.Moderate, id);
            else if (capacity.Moderate < 0)
                _seatRepository.BulkDeleteSeats(capacity.Moderate, (int) SeatTypeDto.Moderate, id);

            if (capacity.Premier > 0)
                _seatRepository.BulkInsertSeats(capacity.Premier, (int) SeatTypeDto.Premier, id);
            else if (capacity.Premier < 0)
                _seatRepository.BulkDeleteSeats(capacity.Premier, (int) SeatTypeDto.Premier, id);
        }

        /**
         * Returns a SeatCapacity object, separated into
         * a count of each SeatType
         */
        public SeatCapacityDto GetSeatCapacitiesFromDb(int venueId)
        {
            var allSeats = _seatRepository
                .Collection()
                .Where(seat => seat.VenueId == venueId)
                .ToList();

            return new SeatCapacityDto
            {
                Budget = allSeats
                    .Count(seat => seat.SeatTypeId == (int) SeatTypeDto.Budget),
                Moderate = allSeats
                    .Count(seat => seat.SeatTypeId == (int) SeatTypeDto.Moderate),
                Premier = allSeats
                    .Count(seat => seat.SeatTypeId == (int) SeatTypeDto.Premier),
                Total = allSeats.Count()
            };
        }

        public SeatCapacityDto GetSeatCapacitiesFromList(ICollection<Seat> seats)
        {
            return new SeatCapacityDto
            {
                Budget = seats
                    .Count(seat => seat.SeatTypeId == (int) SeatTypeDto.Budget),
                Moderate = seats
                    .Count(seat => seat.SeatTypeId == (int) SeatTypeDto.Moderate),
                Premier = seats
                    .Count(seat => seat.SeatTypeId == (int) SeatTypeDto.Premier),
                Total = seats.Count()
            };
        }

        public IEnumerable<Seat> GetSeatsBySeatType(int venueId, SeatTypeDto type)
        {
            return _seatRepository.Collection()
                .Where(seat => seat.VenueId == venueId)
                .Where(seat => seat.SeatTypeId == (int) type)
                .ToList();
        }
    }
}
