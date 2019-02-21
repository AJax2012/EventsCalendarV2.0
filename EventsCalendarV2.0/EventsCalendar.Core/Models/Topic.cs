using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Models
{
    public class Topic : BaseEntity
    {
        [Display(Name = "Topic")]
        public string Name { get; set; }
    }
}