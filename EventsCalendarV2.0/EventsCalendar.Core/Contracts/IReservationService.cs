﻿using System.Collections.Generic;
using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.Contracts
{
    public interface IReservationService
    {
        IEnumerable<SimpleReservation> CombineSimpleReservations(SimpleReservationsByType reservations);
        IEnumerable<SimpleReservation> CreateSimpleReservations(int venueId, SeatType type, decimal price);
        IEnumerable<Reservation> CreateReservations(int budgetAmount, int moderateAmount, int premierAmount);
        void SetNewReservationPrices(int performanceId, ReservationPrices prices);
        SeatCapacity GetSeatsRemaining(int performanceId);
    }
}