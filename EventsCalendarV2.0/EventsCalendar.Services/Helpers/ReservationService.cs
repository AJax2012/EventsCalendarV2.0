using System.Collections.Generic;
using System.Linq;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts.Services;

namespace EventsCalendar.Services.Helpers
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatService _seatUtil;

        public ReservationService(IReservationRepository reservationRepository,
                                  ISeatService seatUtil)
        {
            _reservationRepository = reservationRepository;
            _seatUtil = seatUtil;
        }

        /**
         * Combines Simple Reservations, originally separated by SeatType/Price
         */
        public IEnumerable<SimpleReservation> CombineSimpleReservations(SimpleReservationsByType reservations)
        {
            var all = new List<SimpleReservation>();
            all.AddRange(reservations.BudgetReservations);
            all.AddRange(reservations.ModerateReservations);
            all.AddRange(reservations.PremierReservations);
            return all;
        }

        /**
         * Creates SimpleReservations from seats.
         * SimpleReservations = Price & ID
         */
        public IEnumerable<SimpleReservation> CreateSimpleReservations(int venueId, SeatType type, decimal price)
        {
            var seats = _seatUtil.GetSeatsBySeatType(venueId, type);
            return seats.Select(seat => new SimpleReservation {SeatId = seat.Id, Price = price}).ToList();
        }

        /**
         * Gets reservations by type
         * Takes the amount of reservations required for each type
         * Combines all reservations to IEnumerable<Reservation>
         */ 
        public IEnumerable<Reservation> GetReservations(SeatCapacity capacity, int performanceId)
        {
            var allReservations = _reservationRepository.Collection()
                .Where(res => res.IsTaken == false)
                .Where(res => res.PerformanceId == performanceId)
                .ToList();

            var budgetReservations = allReservations
                .Where(res => res.Seat.SeatType == SeatType.Budget)
                .Take(capacity.Budget);

            var moderateReservations = allReservations
                .Where(res => res.IsTaken == false)
                .Take(capacity.Moderate);

            var premierReservations = allReservations
                .Where(res => res.Seat.SeatType == SeatType.Budget)
                .Take(capacity.Premier);

            var reservations = new List<Reservation>();
            reservations.AddRange(budgetReservations);
            reservations.AddRange(moderateReservations);
            reservations.AddRange(premierReservations);

            return reservations;
        }

        /**
         * Gets all Reservations at a Performance, separated by SeatType
         * Maps each reservation's Price
         * Regroups and returns all Reservations at a Performance into an IEnumerable<Reservation>
         */
        public void SetNewReservationPrices(int performanceId, ReservationPrices prices)
        {
            var budget = new UpdatePricesObject {
                PerformanceId = performanceId,
                Type = SeatType.Budget,
                Price = prices.Budget
            };

            var moderate = new UpdatePricesObject
            {
                PerformanceId = performanceId,
                Type = SeatType.Moderate,
                Price = prices.Moderate
            };

            var premier = new UpdatePricesObject
            {
                PerformanceId = performanceId,
                Type = SeatType.Premier,
                Price = prices.Premier
            };

            _reservationRepository.ChangeReservationPrices(budget);
            _reservationRepository.ChangeReservationPrices(moderate);
            _reservationRepository.ChangeReservationPrices(premier);
        }

        /**
         * pulls seats by performanceId
         * sorts seats by seattype
         * returns SeatCapacity Object
         */ 
        public SeatCapacityDto GetSeatsRemaining(int performanceId)
        {
            var capacity = new SeatCapacityDto();

            var allSeats = _reservationRepository.Collection()
                .Where(res => res.PerformanceId == performanceId)
                .Where(res => res.IsTaken == false)
                .ToList();

            capacity.Budget = allSeats.Count(res => res.Seat.SeatType == SeatType.Budget);
            capacity.Moderate = allSeats.Count(res => res.Seat.SeatType == SeatType.Moderate);
            capacity.Premier = allSeats.Count(res => res.Seat.SeatType == SeatType.Premier);

            return capacity;
        }
    }
}
