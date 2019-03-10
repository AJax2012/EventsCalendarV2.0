using System;
using System.Collections.Generic;

namespace EventsCalendar.Core.Contracts.Repositories
{
    public interface IGuidRepository<T>
    {
        IEnumerable<T> Collection();
        void Commit();
        void Delete(Guid id);
        T Find(Guid id);
        void Insert(T t);
        void Update(T t);
    }
}
