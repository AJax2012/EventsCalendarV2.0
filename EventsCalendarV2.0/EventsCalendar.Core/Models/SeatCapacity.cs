using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCalendar.Core.Models
{
    public class SeatCapacity
    {
        public int Budget { get; set; }
        public int Moderate { get; set; }
        public int Premier { get; set; }
        public int Total { get; set; }
    }
}
