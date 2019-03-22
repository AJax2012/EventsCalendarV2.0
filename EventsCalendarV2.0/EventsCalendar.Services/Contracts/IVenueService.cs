using System.Collections.Generic;
using EventsCalendar.Core.Models;
using EventsCalendar.Services.Dtos.Venue;

namespace EventsCalendar.Services.Contracts
{
    public interface IVenueService
    {
        void CreateVenue(VenueDto venue);
        VenueDto GetVenueDtoById(int id);
        Venue GetVenueById(int id);
        IEnumerable<VenueDto> GetAllVenueDtos();
        IEnumerable<Venue> GetAllVenues();
        void EditVenue(VenueDto venue);
        void DeleteVenue(int id);
    }
}