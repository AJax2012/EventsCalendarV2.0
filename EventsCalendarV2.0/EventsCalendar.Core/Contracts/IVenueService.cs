using System.Collections.Generic;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Core.Contracts
{
    public interface IVenueService
    {
        IEnumerable<VenueViewModel> ListVenues();
        VenueViewModel NewVenueViewModel();
        void CreateVenue(VenueViewModel venueViewModel);
        VenueViewModel ReturnVenueViewModel(int id);
        void EditVenue(VenueViewModel venueViewModel, int id);
        void DeleteVenue(int id);
    }
}