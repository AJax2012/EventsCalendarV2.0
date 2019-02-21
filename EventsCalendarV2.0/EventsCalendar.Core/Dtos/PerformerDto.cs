using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Dtos
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

        public PerformerTypeDto PerformerType { get; set; }

        public GenreDto GenreDto { get; set; }

        //public int CustomImageId { get; set; }

        [Display(Name ="Image")]
        public string ImageUrl { get; set; }

        public TopicDto TopicDto { get; set; }

        public ICollection<PerformanceDto> Performances { get; set; }

        [Display(Name = "Performer Type")]
        public int PerformerTypeId { get; set; }

        [Display(Name = "Genre")]
        public int? GenreId { get; set; }

        [Display(Name = "Topic")]
        public int? TopicId { get; set; }
    }
}
