using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventsCalendar.Core.Models
{
    public sealed class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Performance> Performances { get; set; }

        public Venue()
        {
            Performances = new List<Performance>();
        }
    }
}
