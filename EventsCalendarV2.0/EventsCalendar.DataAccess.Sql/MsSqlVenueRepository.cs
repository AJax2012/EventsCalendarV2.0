﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EventsCalendar.Core.Models;
using EventsCalendar.DataAccess.Sql.Contracts;

namespace EventsCalendar.DataAccess.Sql
{
    public class MsSqlVenueRepository : IRepository<Venue>
    {
        internal DataContext Context;

        public MsSqlVenueRepository(DataContext context)
        {
            Context = context;
        }

        public IEnumerable<Venue> Collection()
        {
            return Context.Venues.Include(v => v.Address).ToList();
        }

        public void Commit()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Delete(int id)
        {
            var venue = Context.Venues
                .Include(v => v.Address)
                .Include(v => v.Seats)
                .Single(v => v.Id == id);

            if (Context.Entry(venue).State == EntityState.Detached)
                Context.Venues.Attach(venue);

            Context.Venues.Remove(venue);
        }

        public Venue Find(int id)
        {
            return Context.Venues
                .Include(v => v.Address)
                .Include(v => v.Seats)
                .SingleOrDefault(v => v.Id == id);
        }

        public void Insert(Venue venue)
        {
            Context.Venues.Add(venue);
        }

        public void Update(Venue venue)
        {
            Context.Venues.Attach(venue);
            Context.Entry(venue).State = EntityState.Modified;
        }

        public void ToggleChangeDetection(bool enabled)
        {
            Context.Configuration.AutoDetectChangesEnabled = enabled;
        }
    }
}
