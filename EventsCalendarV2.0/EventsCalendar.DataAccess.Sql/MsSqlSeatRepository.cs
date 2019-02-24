using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace EventsCalendar.DataAccess.Sql
{
    class MsSqlSeatRepository : IRepository<Seat>
    {
        internal DataContext Context;

        public MsSqlSeatRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Seat> Collection()
        {
            return Context.Seats.ToList();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var seat = Context.Seats.Single(s => s.Id == id);

            if (Context.Entry(seat).State == EntityState.Detached)
                Context.Seats.Attach(seat);

            Context.Seats.Remove(seat);
        }

        public Seat Find(int id)
        {
            return Context.Seats
                .Include(s => s.Venue)
                .SingleOrDefault(s => s.Id == id);
        }

        public void Insert(Seat seat)
        {
            Context.Seats.Add(seat);
        }

        public void Update(Seat seat)
        {
            Context.Seats.Attach(seat);
            Context.Entry(seat).State = EntityState.Modified;
        }
    }
}
