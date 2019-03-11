using System;
using System.Collections.Generic;

namespace EventsCalendar.Services.Contracts.Services
{
    public interface ITicketService
    {
        void CreateTicket(ITicketViewModel ticketViewModel);
        void DeleteTicket(Guid id);
        void EditTicket(ITicketViewModel ticketViewModel);
        IEnumerable<ITicketViewModel> ListTickets(ITicketViewModel viewModel);
        ITicketViewModel NewTicketViewModel(ITicketViewModel viewModel);
        ITicketViewModel ReturnTicketViewModelById(Guid id);
        ITicketViewModel ReturnTicketViewModelByConfirmationNumber(string confirmationNumber);
    }
}