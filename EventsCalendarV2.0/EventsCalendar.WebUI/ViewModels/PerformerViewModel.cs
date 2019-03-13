using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Services.Dtos.Performer;

namespace EventsCalendar.WebUI.ViewModels
{
    public class PerformerViewModel
    {
        public PerformerDto Performer { get; set; }

        public IEnumerable<GenreDto> Genres { get; set; }

        public IEnumerable<TopicDto> Topics { get; set; }

        [Display(Name = "Performer Type")]
        public IEnumerable<PerformerTypeDto> PerformerTypes { get; set; }
    }
}