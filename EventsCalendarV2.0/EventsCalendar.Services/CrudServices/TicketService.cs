using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsCalendar.Services.CrudServices
{
    public class TicketService : ITicketService
    {
        public void CreateTicket(TicketViewModel ticketViewModel)
        {
            throw new NotImplementedException();
        }

        public void DeleteTicket(Guid id)
        {
            throw new NotImplementedException();
        }

        public void EditTicket(TicketViewModel ticketViewModel, Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TicketViewModel> ListTickets()
        {
            throw new NotImplementedException();
        }

        public TicketViewModel NewTicketViewModel()
        {
            throw new NotImplementedException();
        }

        public TicketViewModel ReturnTicketViewModel(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
