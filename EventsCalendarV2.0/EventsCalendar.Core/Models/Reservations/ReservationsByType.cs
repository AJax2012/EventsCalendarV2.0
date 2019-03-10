using System.Collections.Generic;

namespace EventsCalendar.Core.Models.Reservations
{
    public class ReservationsByType
    {
        public IEnumerable<Reservation> BudgetReservations { get; set; }
        public IEnumerable<Reservation> ModerateReservations { get; set; }
        public IEnumerable<Reservation> PremierReservations { get; set; }
    }
}
