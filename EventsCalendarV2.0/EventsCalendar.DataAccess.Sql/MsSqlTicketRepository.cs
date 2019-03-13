using EventsCalendar.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EventsCalendar.DataAccess.Sql.Contracts;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlTicketRepository : ITicketRepository
    {
        internal DataContext Context;

        public MsSqlTicketRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Ticket> Collection()
        {
            return Context.Tickets
                .Include(t => t.Reservations)
                .ToList();
        }

        public void Commit()
        {
                Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var ticket = Context.Tickets
                .Single(t => t.Id == id);

            if (Context.Entry(ticket).State == EntityState.Detached)
                Context.Tickets.Attach(ticket);

            Context.Tickets.Remove(ticket);
        }

        public Ticket Find(Guid id)
        {
            return Context.Tickets
                .Include(t => t.Reservations)
                .SingleOrDefault(t => t.Id == id);
        }

        public Ticket FindByConfirmationNumber(string confirmationNumber)
        {
            return Context.Tickets
                .Include(t => t.Reservations)
                .SingleOrDefault(t => t.ConfirmationNumber == confirmationNumber);
        }

        public void Insert(Ticket ticket)
        {
            Context.Tickets.Add(ticket);
        }

        public void Update(Ticket ticket)
        {
            Context.Tickets.Attach(ticket);
            Context.Entry(ticket).State = EntityState.Modified;
        }

        public void ToggleChangeDetection(bool enabled)
        {
            throw new NotImplementedException();
        }
    }
}
