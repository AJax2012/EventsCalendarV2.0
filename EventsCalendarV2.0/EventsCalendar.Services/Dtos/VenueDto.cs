using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Services.Dtos
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

        public ICollection<SeatDto> Seats { get; set; }
    }
}
