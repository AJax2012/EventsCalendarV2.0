using System.Collections.Generic;

namespace EventsCalendar.Services.Contracts.Services
{
    public interface IVenueService
    {
        void CreateVenue(IVenueViewModel venueViewModel);
        void DeleteVenue(int id);
        void EditVenue(IVenueViewModel venueViewModel, int id);
        IEnumerable<IVenueViewModel> ListVenues();
        IVenueViewModel ReturnVenueViewModel(IVenueViewModel viewModel);
    }
}