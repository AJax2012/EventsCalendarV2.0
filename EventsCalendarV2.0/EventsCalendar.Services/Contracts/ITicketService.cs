using System;
using System.Collections.Generic;
using EventsCalendar.Core.Models;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.Contracts
{
    public interface ITicketService
    {
        void CreateTicket(TicketDto ticket);
        void DeleteTicket(Guid id);
        void EditTicket(TicketDto ticketViewModel);
        ICollection<TicketDto> GetAllTicketDtos();
        ICollection<Ticket> GetAllTickets();
        TicketDto GetTicketDtoById(Guid id);
        Ticket GetTicketById(Guid id);
        TicketDto GetTicketDtoByConfirmationNumber(string confirmationNumber);
        Ticket GetTicketByConfirmationNumber(string confirmationNumber);
    }
}