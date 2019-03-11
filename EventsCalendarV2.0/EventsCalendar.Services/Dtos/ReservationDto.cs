﻿using System;

namespace EventsCalendar.Services.Dtos
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int SeatId { get; set; }
        public SeatDto Seat { get; set; }
        public int PerformanceID { get; set; }
        public PerformanceDto Performance { get; set; }
        public int? TicketId { get; set; }
        public TicketDto Ticket { get; set; }
        public bool IsTaken { get; set; }
    }
}