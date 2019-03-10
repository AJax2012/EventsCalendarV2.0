using System.Collections.Generic;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Contracts.Services
{
    public interface IVenueService
    {
        void CreateVenue(VenueViewModel venueViewModel);
        void DeleteVenue(int id);
        void EditVenue(VenueViewModel venueViewModel, int id);
        IEnumerable<VenueViewModel> ListVenues();
        VenueViewModel NewVenueViewModel();
        VenueViewModel ReturnVenueViewModel(int id);
    }
}