using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Dtos
{
    public class TopicDto
    {
        [Display(Name = "Topic")]
        public int Id { get; set; }

        [Display(Name = "Topic")]
        public string Name { get; set; }
    }
}
