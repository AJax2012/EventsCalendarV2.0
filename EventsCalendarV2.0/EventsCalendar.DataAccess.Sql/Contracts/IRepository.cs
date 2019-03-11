using System.Collections.Generic;

namespace EventsCalendar.DataAccess.Sql.Contracts
{
    public interface IRepository<T>
    {
        IEnumerable<T> Collection();
        void Commit();
        void Delete(int id);
        T Find(int id);
        void Insert(T t);
        void Update(T t);
    }
}