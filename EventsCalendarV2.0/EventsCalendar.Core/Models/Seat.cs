using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCalendar.Core.Models
{
    public class Seat : BaseEntity
    {
        public decimal Price { get; set; }
        public SeatType SeatType { get; set; }
    }
}
