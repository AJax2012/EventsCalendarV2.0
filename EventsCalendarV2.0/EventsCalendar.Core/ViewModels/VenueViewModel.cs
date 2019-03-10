using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.ViewModels
{
    public class VenueViewModel
    {
        public VenueDto Venue { get; set; }
        public SeatCapacity SeatCapacity { get; set; }
        public string ImgSrc { get; set; }
    }
}
