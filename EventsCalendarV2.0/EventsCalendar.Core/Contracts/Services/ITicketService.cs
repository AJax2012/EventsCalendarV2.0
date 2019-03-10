using System;
using System.Collections.Generic;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Contracts.Services
{
    public interface ITicketService
    {
        void CreateTicket(TicketViewModel ticketViewModel);
        void DeleteTicket(Guid id);
        void EditTicket(TicketViewModel ticketViewModel);
        IEnumerable<TicketViewModel> ListTickets();
        NewTicketViewModel NewTicketViewModel(int performanceId);
        TicketViewModel ReturnTicketViewModel(Guid id);
    }
}