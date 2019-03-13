using System.Collections.Generic;

namespace EventsCalendar.Services.Dtos.Reservation
{
    public class ReservationsByType
    {
        public IEnumerable<Core.Models.Reservations.Reservation> BudgetReservations { get; set; }
        public IEnumerable<Core.Models.Reservations.Reservation> ModerateReservations { get; set; }
        public IEnumerable<Core.Models.Reservations.Reservation> PremierReservations { get; set; }
    }
}
