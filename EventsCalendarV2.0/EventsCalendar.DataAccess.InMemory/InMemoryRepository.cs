using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using EventsCalendar.Core.Models;

namespace EventsCalendar.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly List<T> _items;
        private readonly string _className;

        public InMemoryRepository()
        {
            _className = typeof(T).Name;
            _items = _cache[_className] as List<T> ?? new List<T>();
        }
        
        public IEnumerable<T> Collection()
        {
            return _items.AsQueryable();
        }

        public void Commit()
        {
            _cache[_className] = _items;
        }

        public void Delete(int id)
        {
            T tToDelete = _items.Find(i => i.Id == id);

            if (tToDelete == null)
                throw new Exception(_className + " Not Found");

            _items.Remove(tToDelete);
        }

        public T Find(int id)
        {
            T t = _items.Find(i => i.Id == id);

            if (t == null)
                throw new Exception(_className + " Not Found");

            return t;
        }

        public void Insert(T t)
        {
            _items.Add(t);
        }

        public void ToggleChangeDetection(bool enabled)
        {
            throw new NotImplementedException();
        }

        public void Update(T performer)
        {
            T tToUpdate = _items.Find(i => i.Id == performer.Id);
            var index = _items.FindIndex(i => i.Id == performer.Id);

            if (tToUpdate == null)
                throw new Exception(_className + " Not Found");

            _items[index] = performer;
        }
    }
}
