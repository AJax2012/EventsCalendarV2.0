using System.Collections.Generic;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.WebUI.ViewModels
{
    public class PerformerViewModel : IPerformerViewModel
    {
        public PerformerDto Performer { get; set; }
        public IEnumerable<GenreDto> Genres { get; set; }
        public IEnumerable<TopicDto> Topics { get; set; }
        public IEnumerable<PerformerTypeDto> PerformerTypes { get; set; }
    }
}