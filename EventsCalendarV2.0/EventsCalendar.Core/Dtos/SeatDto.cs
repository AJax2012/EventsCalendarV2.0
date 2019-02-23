using EventsCalendar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCalendar.Core.Dtos
{
    public class SeatDto
    {
        public decimal Price { get; set; }
        public SeatType SeatType { get; set; }
        public int PerformanceId { get; set; }
        public PerformanceDto PerformanceDto { get; set; }
    }
}
