﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlPerformanceRepository : IRepository<Performance>
    {
        internal DataContext Context;
        private IReservationRepository _reservationRepository;

        public MsSqlPerformanceRepository(DataContext context, IReservationRepository reservationRepository)
        {
            Context = context;
            _reservationRepository = reservationRepository;
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
            bool fileSaved;
            do
            {
                fileSaved = false;
                try
                {
                    Context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    fileSaved = true;
                    var entry = e.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            } while (fileSaved);
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
