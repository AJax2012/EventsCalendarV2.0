using System.Collections.Generic;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.Services.Dtos.Seat;

namespace EventsCalendar.Services.Contracts
{
    public interface ISeatService
    {
        SeatCapacityDto CalculateAmountOfSeatsToChange(SeatCapacityDto oldSeats, SeatCapacityDto newSeats);
        void ChangeAmountOfSeatsInContext(SeatCapacityDto capacity, int id);
        SeatCapacityDto GetSeatCapacitiesFromDb(int venueId);
        SeatCapacityDto GetSeatCapacitiesFromList(ICollection<Seat> seats);
        IEnumerable<Seat> GetSeatsBySeatType(int venueId, SeatTypeDto type);
    }
}