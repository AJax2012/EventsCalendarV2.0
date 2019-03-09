using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCalendar.Core.Models
{
    public class ConfirmationNumberData
    {
        public char VenueChar { get; set; }
        public char VenueRandom { get; set; }
        public char SeatDigit { get; set; }
        public char PerformerChar { get; set; }
        public char FirstReservationChar { get; set; }
        public char LastReservationChar { get; set; }
        public char RandomReservationChar { get; set; }
    }
}
