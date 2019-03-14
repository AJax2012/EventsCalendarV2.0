using System.Collections.Generic;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Seat;

namespace EventsCalendar.Services.Contracts
{
    public interface IReservationService
    {
        IEnumerable<SimpleReservation> CombineSimpleReservations(SimpleReservationsByType reservations);
        IEnumerable<SimpleReservation> CreateSimpleReservations(int venueId, SeatTypeDto type, decimal price);
        void InsertReservations(IEnumerable<SimpleReservation> reservations, int performanceId);
        SeatCapacityDto GetSeatsRemaining(int performanceId);
        IEnumerable<Reservation> GetReservations(SeatCapacityDto capacity, int performanceId);
        IEnumerable<ReservationDto> GetReservationDtos(SeatCapacityDto capacity, int performanceId);
        void SetNewReservationPrices(int performanceId, ReservationPrices prices);
        ReservationPricesDto GetPrices(int performanceId);
    }
}