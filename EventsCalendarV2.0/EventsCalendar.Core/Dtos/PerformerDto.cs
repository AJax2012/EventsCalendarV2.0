using EventsCalendar.Core.Models;
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

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public Genre Genre { get; set; }

        public Topic Topic { get; set; }

        public PerformerType PerformerType { get; set; }

        public ICollection<PerformanceDto> Performances { get; set; }
    }
}
