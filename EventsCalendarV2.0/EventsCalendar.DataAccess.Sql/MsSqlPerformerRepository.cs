using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Models;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlPerformerRepository : IRepository<Performer>
    {
        internal DataContext Context;

        public MsSqlPerformerRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Performer> Collection()
        {
            return Context.Performers
                .Include(p => p.PerformerType)
                .Include(p => p.Genre)
                .Include(p => p.Topic)
                .ToList();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var performer = Context.Performers.Single(v => v.Id == id);

            if (Context.Entry(performer).State == EntityState.Detached)
                Context.Performers.Attach(performer);

            Context.Performers.Remove(performer);
        }

        public Performer Find(int id)
        {
            return Context.Performers
                .Include(p => p.PerformerType)
                .Include(p => p.Genre)
                .Include(p => p.Topic)
                .SingleOrDefault(v => v.Id == id);
        }

        public void Insert(Performer performer)
        {
            Context.Performers.Add(performer);
        }

        public void Update(Performer performer)
        {
            Context.Performers.Attach(performer);
            Context.Entry(performer).State = EntityState.Modified;
        }
    }
}
