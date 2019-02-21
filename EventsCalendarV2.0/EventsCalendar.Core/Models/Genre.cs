using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Models
{
    public class Genre : BaseEntity
    {
        [Display(Name = "Genre")]
        public string Name { get; set; }
    }
}