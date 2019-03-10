using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Contracts.Repositories;
using EventsCalendar.Core.Models;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlPerformanceRepository : IRepository<Performance>
    {
        internal DataContext Context;

        public MsSqlPerformanceRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Performance> Collection()
        {
            return Context.Performances
                .Include(p => p.Performer)
                .Include(p => p.Venue)
                .Include(p => p.Reservations)
                .ToList();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var performance = Context.Performances.Single(v => v.Id == id);

            if (Context.Entry(performance).State == EntityState.Detached)
                Context.Performances.Attach(performance);

            Context.Performances.Remove(performance);
        }

        public Performance Find(int id)
        {
            return Context.Performances
                .Include(p => p.Performer)
                .Include(p => p.Venue)
                .Include(p => p.Reservations)
                .SingleOrDefault(v => v.Id == id);
        }

        public void Insert(Performance performance)
        {
            Context.Performances.Add(performance);
        }

        public void Update(Performance performance)
        {
            Context.Performances.Attach(performance);
            Context.Entry(performance).State = EntityState.Modified;
        }

        public void ToggleChangeDetection(bool enabled)
        {
            Context.Configuration.AutoDetectChangesEnabled = enabled;
        }
    }
}
