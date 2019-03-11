using EventsCalendar.Core.Models.Tickets;

namespace EventsCalendar.DataAccess.Sql.Contracts
{
    public interface ITicketRepository : IGuidRepository<Ticket>
    {
        Ticket FindByConfirmationNumber(string confirmationNumber);
    }
}