using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlReservationRepository : IGuidRepository<Reservation>
    {
        internal DataContext Context;

        public MsSqlReservationRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Reservation> Collection()
        {
            return Context.Reservations.ToList();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var reservation = Context.Reservations.Single(r => r.Id == id);

            if (Context.Entry(reservation).State == EntityState.Detached)
                Context.Reservations.Attach(reservation);

            Context.Reservations.Remove(reservation);
        }

        public Reservation Find(Guid id)
        {
            return Context.Reservations
                .Include(r => r.Performance)
                .Include(r => r.Seat)
                .SingleOrDefault(r => r.Id == id);
        }

        public void Insert(Reservation reservation)
        {
            Context.Reservations.Add(reservation);
        }

        public void Update(Reservation reservation)
        {
            Context.Reservations.Attach(reservation);
            Context.Entry(reservation).State = EntityState.Modified;
        }

        public void ToggleChangeDetection(bool enabled)
        {
            Context.Configuration.AutoDetectChangesEnabled = enabled;
        }
    }
}
