using System;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.Contracts
{
    public interface ITicketService
    {
        void CreateTicket(TicketDto ticket);
        void DeleteTicket(Guid id);
        void EditTicket(TicketDto ticketViewModel);
    }
}