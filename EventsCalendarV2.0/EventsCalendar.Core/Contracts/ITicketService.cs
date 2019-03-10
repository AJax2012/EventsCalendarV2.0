using System;
using System.Collections.Generic;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Contracts
{
    public interface ITicketService
    {
        IEnumerable<TicketViewModel> ListTickets();
        NewTicketViewModel NewTicketViewModel(int performanceId);
        void CreateTicket(TicketViewModel ticketViewModel);
        TicketViewModel ReturnTicketViewModel(Guid id);
        void EditTicket(TicketViewModel ticketViewModel);
        void DeleteTicket(Guid id);
    }
}