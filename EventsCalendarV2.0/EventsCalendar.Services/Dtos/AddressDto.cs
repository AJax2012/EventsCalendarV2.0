using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Services.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }

        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        public bool IsActive { get; set; }
    }
}
