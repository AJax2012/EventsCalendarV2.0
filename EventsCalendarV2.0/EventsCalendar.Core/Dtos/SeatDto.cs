using System.Collections.Generic;

namespace EventsCalendar.Core.Dtos
{
    public class SeatDto
    {
        public int Id { get; set; }
        public SeatTypeDto SeatType { get; set; }
        public int PerformanceId { get; set; }
        public PerformanceDto PerformanceDto { get; set; }
        public ICollection<ReservationDto> ReservationDtos { get; set; }
    }
}
