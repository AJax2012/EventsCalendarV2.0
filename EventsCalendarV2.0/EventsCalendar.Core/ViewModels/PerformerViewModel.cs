using System.Collections.Generic;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models;

namespace EventsCalendar.Core.ViewModels
{
    public class PerformerViewModel
    {
        public PerformerDto Performer { get; set; }
        public string ImgSrc { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Topic> Topics { get; set; }
        public IEnumerable<PerformerType> PerformerTypes { get; set; }
    }
}