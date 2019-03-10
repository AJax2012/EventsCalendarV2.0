using System.Collections.Generic;
using EventsCalendar.Core.Models.Seats;

namespace EventsCalendar.Core.Models
{
    public class Venue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Performance> Performances { get; set; }
        public ICollection<Seat> Seats { get; set; }

        public Venue()
        {
            Performances = new List<Performance>();
            Seats = new List<Seat>();
        }
    }
}
