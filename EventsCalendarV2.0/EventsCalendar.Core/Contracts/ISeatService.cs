using EventsCalendar.Core.Dtos;
using System.Collections.Generic;

namespace EventsCalendar.Core.Contracts
{
    public interface ISeatService
    {
        IEnumerable<SeatDto> ListSeats();
        SeatDto NewSeatDto();
        void CreateSeat(SeatDto seatDto);
        SeatDto ReturnSeatDto(int id);
        SeatDto ReturnSeatDetails(int id);
        void EditSeat(SeatDto seatDto, int id);
        void DeleteSeat(int id);
    }
}
