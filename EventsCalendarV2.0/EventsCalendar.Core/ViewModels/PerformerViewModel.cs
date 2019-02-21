using System.Collections.Generic;
using System.Web;
using EventsCalendar.Core.Dtos;

namespace EventsCalendar.Core.ViewModels
{
    public class PerformerViewModel
    {
        public IEnumerable<PerformerTypeDto> PerformerTypes { get; set; }

        //public HttpPostedFileBase Image { get; set; }
        public string ImgSrc { get; set; }

        public IEnumerable<GenreDto> Genres { get; set; }

        public IEnumerable<TopicDto> Topics { get; set; }

        public PerformerDto Performer { get; set; }
    }
}