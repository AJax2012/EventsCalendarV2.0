using System.Collections.Generic;
using System.Data.Entity;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Contracts.Repositories;
using EventsCalendar.Core.Models;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlGenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext Context;
        internal DbSet<T> DbSet;

        public MsSqlGenericRepository()
        {
            Context = new DataContext();
            DbSet = Context.Set<T>();
        }

        public IEnumerable<T> Collection()
        {
            return DbSet;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var t = Find(id);
            if (Context.Entry(t).State == EntityState.Detached)
                DbSet.Attach(t);

            DbSet.Remove(t);
        }

        public T Find(int id)
        {
            return DbSet.Find(id);
        }

        public void Insert(T t)
        {
            DbSet.Add(t);
        }

        public void Update(T performer)
        {
            DbSet.Attach(performer);
            Context.Entry(performer).State = EntityState.Modified;
        }

        public void ToggleChangeDetection(bool enabled)
        {
            Context.Configuration.AutoDetectChangesEnabled = enabled;
        }
    }
}
