using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Services.Dtos.Performer
{
    public class PerformerDto
    {
        public int Id { get; set; }

        [Display(Name = "Performer")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Tour Name")]
        public string TourName { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public GenreDto Genre { get; set; }

        public TopicDto Topic { get; set; }

        public PerformerTypeDto PerformerType { get; set; }

        public ICollection<PerformanceDto> Performances { get; set; }
    }
}
