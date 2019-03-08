using System;
using System.Collections.Generic;
using System.Web;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Contracts
{
    public interface ITicketService
    {
        IEnumerable<TicketViewModel> ListTickets();
        TicketViewModel NewTicketViewModel();
        void CreateTicket(TicketViewModel ticketViewModel);
        TicketViewModel ReturnTicketViewModel(Guid id);
        void EditTicket(TicketViewModel ticketViewModel, Guid id);
        void DeleteTicket(Guid id);
    }
}