using System.Collections.Generic;
using EventsCalendar.Core.Models.Reservations;

namespace EventsCalendar.Services.Dtos.Reservation
{
    public class SimpleReservationsByType
    {
        public IEnumerable<SimpleReservation> BudgetReservations { get; set; }
        public IEnumerable<SimpleReservation> ModerateReservations { get; set; }
        public IEnumerable<SimpleReservation> PremierReservations { get; set; }
    }
}
