using System.Collections.Generic;

namespace EventsCalendar.Core.Models.Reservations
{
    public class SimpleReservationsByType
    {
        public IEnumerable<SimpleReservation> BudgetReservations { get; set; }
        public IEnumerable<SimpleReservation> ModerateReservations { get; set; }
        public IEnumerable<SimpleReservation> PremierReservations { get; set; }
    }
}
