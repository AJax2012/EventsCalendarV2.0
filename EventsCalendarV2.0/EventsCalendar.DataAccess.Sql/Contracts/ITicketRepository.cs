using EventsCalendar.Core.Models;

namespace EventsCalendar.DataAccess.Sql.Contracts
{
    public interface ITicketRepository : IGuidRepository<Ticket>
    {
        Ticket FindByConfirmationNumber(string confirmationNumber);
    }
}