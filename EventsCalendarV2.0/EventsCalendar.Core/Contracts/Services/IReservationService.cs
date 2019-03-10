﻿using System.Collections.Generic;
using EventsCalendar.Core.Models.Reservations;
using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.Core.Contracts.Services
{
    public interface IReservationService
    {
        IEnumerable<SimpleReservation> CombineSimpleReservations(SimpleReservationsByType reservations);
        IEnumerable<SimpleReservation> CreateSimpleReservations(int venueId, SeatType type, decimal price);
        SeatCapacity GetSeatsRemaining(int performanceId);
        IEnumerable<Reservation> GetReservations(SeatCapacity capacity, int performanceId);
        void SetNewReservationPrices(int performanceId, ReservationPrices prices);
    }
}