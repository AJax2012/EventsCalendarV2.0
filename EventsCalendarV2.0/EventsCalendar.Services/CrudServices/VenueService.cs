using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Contracts;
using EventsCalendar.Core.Dtos;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.ViewModels;

namespace EventsCalendar.Services.CrudServices
{
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> _context;
        private readonly IRepository<Address> _addressContext;
        private readonly IRepository<Performance> _performanceContext;
        private readonly string DefaultImgSrc = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJa4VlErDGxyBl-tQu41odZDe-qLvI1xNDALRMYxTITZOb3DslFg";

        public VenueService(IRepository<Venue> context, 
                            IRepository<Address> addressContext, 
                            IRepository<Performance> performanceContext)
        {
            _context = context;
            _addressContext = addressContext;
            _performanceContext = performanceContext;
        }

        private Venue FindVenueDto(int id)
        {
            return _context.Find(id);
        }

        private Venue CheckVenueNullValue(int id)
        {
            Venue venue = FindVenueDto(id);
            if (venue == null)
                throw new HttpException(404, "Venue Not Found");

            return venue;
        }

        public IEnumerable<VenueViewModel> ListVenues()
        {
            IEnumerable<Venue> venues = _context.Collection().ToList();

            var venueDtos =
                Mapper.Map<IEnumerable<Venue>, IEnumerable<VenueDto>>
                    (venues);

            var venueViewModels =
                Mapper.Map<IEnumerable<VenueDto>, 
                    IEnumerable<VenueViewModel>>(venueDtos);
            
            return venueViewModels;
        }

        public VenueViewModel NewVenueViewModel()
        {
            VenueViewModel viewModel = new VenueViewModel
            {
                Venue = new VenueDto(),
                ImgSrc = DefaultImgSrc
            };

            viewModel.Venue.Id = 0;
            viewModel.Venue.AddressId = 0;

            return viewModel;
        }

        public void CreateVenue(VenueViewModel venueViewModel)
        {
            var venue = new Venue
            {
                Capacity = venueViewModel.Venue.Capacity,
                IsActive = true,
                Name = venueViewModel.Venue.Name,
                ImageUrl = venueViewModel.Venue.ImageUrl,

                Address = new Address
                {
                    StreetAddress = venueViewModel.Venue.AddressDto.StreetAddress,
                    City = venueViewModel.Venue.AddressDto.City,
                    State = venueViewModel.Venue.AddressDto.State,
                    ZipCode = venueViewModel.Venue.AddressDto.ZipCode,
                }
            };

            if (string.IsNullOrWhiteSpace(venue.ImageUrl))
                venue.ImageUrl = DefaultImgSrc;

            _context.Insert(venue);
            _context.Commit();
        }

        public VenueViewModel ReturnVenueViewModel(int id)
        {
            Venue venue = CheckVenueNullValue(id);
            venue.Address = _addressContext.Find(venue.AddressId);

            var viewModel = new VenueViewModel
            {
                Venue = Mapper.Map<Venue, VenueDto>(venue),
                ImgSrc = venue.ImageUrl,
            };

            return viewModel;
        }

        public void EditVenue(VenueViewModel venueViewModel, int id)
        {
            Venue venueToEdit = CheckVenueNullValue(id);

            venueToEdit.AddressId = venueViewModel.Venue.AddressId;
            venueToEdit.Capacity = venueViewModel.Venue.Capacity;
            venueToEdit.Name = venueViewModel.Venue.Name;
            venueToEdit.ImageUrl = venueViewModel.Venue.ImageUrl;
            venueToEdit.Address.StreetAddress = venueViewModel.Venue.AddressDto.StreetAddress;
            venueToEdit.Address.City = venueViewModel.Venue.AddressDto.City;
            venueToEdit.Address.State = venueViewModel.Venue.AddressDto.State;
            venueToEdit.Address.ZipCode = venueViewModel.Venue.AddressDto.ZipCode;
            venueToEdit.IsActive = true;

            _context.Commit();
            _addressContext.Commit();
        }

        public void DeleteVenue(int id)
        {
            var venue = CheckVenueNullValue(id);

            List<Performance> performances = 
                _performanceContext.Collection()
                    .Where(p => p.VenueId == id).ToList();

//            venue.IsActive = false;
//            venue.Address.IsActive = false;
           
            foreach (var performance in performances)
            {
                _performanceContext.Delete(performance.Id);
                _performanceContext.Commit();
            }

            _context.Delete(id);
            _addressContext.Delete(venue.AddressId);
            _context.Commit();
            _addressContext.Commit();
        }
    }
}
