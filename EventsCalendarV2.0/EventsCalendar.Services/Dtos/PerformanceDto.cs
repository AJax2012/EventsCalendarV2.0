﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Dtos.Reservation;
using EventsCalendar.Services.Dtos.Venue;

namespace EventsCalendar.Services.Dtos
{
    public class PerformanceDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ReservationPricesDto Prices { get; set; }

        [Display(Name = "Event Date and Time")]
        public DateTime EventDateTime { get; set; }

        public PerformerDto PerformerDto { get; set; }

        public VenueDto VenueDto { get; set; }

        public ICollection<ReservationDto> Reservations { get; set; }
    }
}
