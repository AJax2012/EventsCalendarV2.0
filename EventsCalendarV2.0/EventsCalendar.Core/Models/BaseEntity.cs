using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EventsCalendar.Core.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}