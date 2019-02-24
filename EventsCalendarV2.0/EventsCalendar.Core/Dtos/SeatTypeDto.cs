using EventsCalendar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCalendar.Core.Dtos
{
    public class SeatTypeDto
    {
        public SeatTypeLevels SeatTypeLevels { get; set; }
        public decimal? Price { get; set; }
        public int NumberOfSeats { get; set; }
    }
}
