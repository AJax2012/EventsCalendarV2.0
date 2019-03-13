using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventsCalendar.Services.Dtos.Performer;
using EventsCalendar.Services.Dtos.Seat;

namespace EventsCalendar.Services.Dtos.Venue
{
    public class VenueDto
    {
        [Display(Name = "Venue")]
        public int Id { get; set; }

        [Display(Name = "Venue")]
        public string Name { get; set; }

        [Display(Name="Image")]
        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Address")]
        public int AddressId { get; set; }

        public AddressDto AddressDto { get; set; }

        public ICollection<PerformanceDto> Performances { get; set; }

        public SeatCapacityDto SeatCapacity { get; set; }
    }
}
