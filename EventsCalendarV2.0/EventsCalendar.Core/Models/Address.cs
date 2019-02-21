using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Models
{
    public class Address : BaseEntity
    {
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public bool IsActive { get; set; }
    }
}
