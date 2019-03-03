using System.Collections.Generic;

namespace EventsCalendar.Core.Contracts
{
    public interface IRepository<T>
    {
        IEnumerable<T> Collection();
        void Commit();
        void Delete(int id);
        T Find(int id);
        void Insert(T t);
        void Update(T t);
        void ToggleChangeDetection(bool enabled);
    }
}