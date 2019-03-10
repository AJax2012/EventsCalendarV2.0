using EventsCalendar.Core.Models.Tickets;

namespace EventsCalendar.Core.Contracts.Repositories
{
    public interface ITicketRepository : IGuidRepository<Ticket>
    {
        Ticket FindByConfirmationNumber(string confirmationNumber);
    }
}