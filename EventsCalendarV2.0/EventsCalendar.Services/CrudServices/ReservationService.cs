using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Seat;

namespace EventsCalendar.Services.CrudServices
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly ISeatService _seatService;

        public ReservationService(IReservationRepository repository,
                                  ISeatService seatService)
        {
            _repository = repository;
            _seatService = seatService;
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
        public IEnumerable<SimpleReservation> CreateSimpleReservations(int venueId, SeatTypeDto type, decimal price)
        {
            var seats = _seatService.GetSeatsBySeatType(venueId, type);
            return seats.Select(seat => new SimpleReservation {SeatId = seat.Id, Price = price}).ToList();
        }

        /**
         * Gets reservations by type
         * Takes the amount of reservations required for each type
         * Combines all reservations to IEnumerable<Reservation>
         */ 
        public IEnumerable<Reservation> GetReservations(SeatCapacityDto capacity, int performanceId)
        {
            var allReservations = _repository.Collection()
                .Where(res => res.IsTaken == false)
                .Where(res => res.PerformanceId == performanceId)
                .ToList();

            var budgetReservations = allReservations
                .Where(res => res.Seat.SeatTypeId == (int) SeatTypeDto.Budget)
                .Take(capacity.Budget);

            var moderateReservations = allReservations
                .Where(res => res.Seat.SeatTypeId == (int) SeatTypeDto.Moderate)
                .Take(capacity.Moderate);

            var premierReservations = allReservations
                .Where(res => res.Seat.SeatTypeId == (int) SeatTypeDto.Premier)
                .Take(capacity.Premier);

            var reservations = new List<Reservation>();
            reservations.AddRange(budgetReservations);
            reservations.AddRange(moderateReservations);
            reservations.AddRange(premierReservations);

            return reservations;
        }

        /**
         * Gets reservations by type
         * Takes the amount of reservations required for each type
         * Combines all reservations to IEnumerable<Reservation>
         */
        public IEnumerable<ReservationDto> GetReservationDtos(SeatCapacityDto capacity, int performanceId)
        {
            var reservations = Mapper.Map
                <IEnumerable<Reservation>, IEnumerable<ReservationDto>>
                (GetReservations(capacity, performanceId));

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
                SeatTypeId = (int) SeatTypeDto.Budget,
                Price = prices.Budget
            };

            var moderate = new UpdatePricesObject
            {
                PerformanceId = performanceId,
                SeatTypeId = (int)SeatTypeDto.Moderate,
                Price = prices.Moderate
            };

            var premier = new UpdatePricesObject
            {
                PerformanceId = performanceId,
                SeatTypeId = (int) SeatTypeDto.Premier,
                Price = prices.Premier
            };

            _repository.ChangeReservationPrices(budget);
            _repository.ChangeReservationPrices(moderate);
            _repository.ChangeReservationPrices(premier);
        }

        /**
         * pulls seats by performanceId
         * sorts seats by seat type
         * returns SeatCapacity Object
         */ 
        public SeatCapacityDto GetSeatsRemaining(int performanceId)
        {
            var capacity = new SeatCapacityDto();

            var allSeats = _repository.Collection()
                .Where(res => res.PerformanceId == performanceId)
                .Where(res => res.IsTaken == false)
                .ToList();

            capacity.Budget = allSeats.Count(res => res.Seat.SeatTypeId == (int) SeatTypeDto.Budget);
            capacity.Moderate = allSeats.Count(res => res.Seat.SeatTypeId == (int) SeatTypeDto.Moderate);
            capacity.Premier = allSeats.Count(res => res.Seat.SeatTypeId == (int) SeatTypeDto.Premier);

            return capacity;
        }

        public void InsertReservations(IEnumerable<SimpleReservation> reservations, int performanceId)
        {
            _repository.BulkInsertReservations(reservations, performanceId);
        }

        public ReservationPricesDto GetPrices(int performanceId)
        {
            return Mapper.Map
                <ReservationPrices, ReservationPricesDto>
                (_repository.GetPrices(performanceId));
        }
    }
}
