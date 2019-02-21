using EventsCalendar.Core.Dtos;
using System;

namespace EventsCalendar.Core.ViewModels
{
    public class VenueViewModel
    {
        public VenueDto Venue { get; set; }
        public string ImgSrc { get; set; }
        public string GoogleMapsSrcUrl { get; set; }
    }
}
