using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EventsCalendar.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatService seatUtil;

        public ReservationService(IReservationRepository reservationRepository,
                               ISeatService _seatUtil)
        {
            _reservationRepository = reservationRepository;
            seatUtil = _seatUtil;
        }

        /**
         * Combines Reservations, originally separated by SeatType/Price
         */
        public IEnumerable<SimpleReservation> CombineReservations(IEnumerable<SimpleReservation> budget, IEnumerable<SimpleReservation> moderate, IEnumerable<SimpleReservation> premier)
        {
            List<SimpleReservation> all = new List<SimpleReservation>();
            all.AddRange(budget);
            all.AddRange(moderate);
            all.AddRange(premier);
            return all;
        }

        /**
         * Creates SimpleReservations from seats.
         * SimpleReservations = Price & ID
         */
        public IEnumerable<SimpleReservation> CreateSimpleReservations(int venueId, SeatType type, decimal price)
        {
            IEnumerable<Seat> seats = seatUtil.GetSeatsBySeatType(venueId, type);
            List<SimpleReservation> reservations = new List<SimpleReservation>();

            foreach (var seat in seats)
            {
                reservations.Add(new SimpleReservation
                {
                    SeatId = seat.Id,
                    Price = price
                });
            }

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
         * Retrieves all Reservations at a Performance
         */
        private IEnumerable<Reservation> GetReservations(int performanceId)
        {
            return _reservationRepository.Collection()
                .Where(res => res.PerformanceId == performanceId)
                .ToList();
        }
    }
}
