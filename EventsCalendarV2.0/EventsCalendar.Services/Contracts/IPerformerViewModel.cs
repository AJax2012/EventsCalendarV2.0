using System.Collections.Generic;
using EventsCalendar.Core.Models;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.Contracts
{
    public interface IPerformerViewModel
    {
        PerformerDto Performer { get; set; }
        IEnumerable<GenreDto> Genres { get; set; }
        IEnumerable<TopicDto> Topics { get; set; }
        IEnumerable<PerformerTypeDto> PerformerTypes { get; set; }
    }
}