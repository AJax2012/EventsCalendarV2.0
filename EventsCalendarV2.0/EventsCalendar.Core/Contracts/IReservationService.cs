using EventsCalendar.Core.Dtos;
using System;
using System.Collections.Generic;

namespace EventsCalendar.Core.Contracts
{
    interface IReservationService
    {
        IEnumerable<ReservationDto> ListReservations();
        ReservationDto NewReservationDto();
        void CreateReservation(ReservationDto ReservationDto);
        ReservationDto ReturnReservationDto(Guid id);
        ReservationDto ReturnReservationDetails(Guid id);
        void EditReservation(ReservationDto ReservationDto, Guid id);
        void DeleteReservation(Guid id);
    }
}
