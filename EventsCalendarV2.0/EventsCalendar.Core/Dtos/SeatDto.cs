﻿using EventsCalendar.Core.Models;
using System.Collections.Generic;
using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.Core.Dtos
{
    public class SeatDto
    {
        public int Id { get; set; }
        public SeatType SeatType { get; set; }
        public VenueDto VenueDto { get; set; }
        public ICollection<ReservationDto> Reservations { get; set; }
    }
}
