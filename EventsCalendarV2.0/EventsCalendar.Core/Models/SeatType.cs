﻿using System.Collections.Generic;

namespace EventsCalendar.Core.Models
{
    public class SeatType : BaseEntity
    {
        public SeatTypeLevels SeatTypeLevels { get; set; }
        public decimal? Price { get; set; }
        public int NumberOfSeats { get; set; }
    }
}