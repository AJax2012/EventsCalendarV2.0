﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using EventsCalendar.Core.Models;
using EventsCalendar.Core.Models.Seats;
using EventsCalendar.DataAccess.Sql.Contracts;
using EventsCalendar.Services.Contracts;
using EventsCalendar.Services.Contracts.Services;
using EventsCalendar.Services.Dtos;

namespace EventsCalendar.Services.CrudServices
{
    public class VenueService : IVenueService
    {
        private readonly IRepository<Venue> _repository;
        private readonly IRepository<Address> _addressRepository;
        private readonly IRepository<Performance> _performanceRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ISeatService _seatService;
        private const string DefaultImgSrc = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTJa4VlErDGxyBl-tQu41odZDe-qLvI1xNDALRMYxTITZOb3DslFg";

        public VenueService(IRepository<Venue> repo, 
                            IRepository<Address> addressRepo, 
                            ISeatRepository seatRepo,
                            IRepository<Performance> performanceRepo,
                            ISeatService seatService)
        {
            _repository = repo;
            _addressRepository = addressRepo;
            _seatRepository = seatRepo;
            _performanceRepository = performanceRepo;
            _seatService = seatService;
        }

        private Venue CheckVenueNullValue(int id)
        {
            Venue venue = _repository.Find(id);
            if (venue == null)
                throw new HttpException(404, "Venue Not Found");

            return venue;
        }

        public IEnumerable<IVenueViewModel> ListVenues()
        {
            IEnumerable<Venue> venues = _repository.Collection().ToList();

            var venueDtos =
                Mapper.Map<IEnumerable<Venue>, IEnumerable<VenueDto>>
                    (venues);

            var venueViewModels =
                Mapper.Map<IEnumerable<VenueDto>, 
                    IEnumerable<IVenueViewModel>>(venueDtos);
            
            return venueViewModels;
        }

        public void CreateVenue(IVenueViewModel venueViewModel)
        {
            var venue = new Venue
            {
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

            _repository.Insert(venue);
            _repository.Commit();

            var budget = venueViewModel.SeatCapacity.Budget;
            var moderate = venueViewModel.SeatCapacity.Moderate;
            var premier = venueViewModel.SeatCapacity.Premier;

            _seatRepository.BulkInsertSeats(budget, SeatType.Budget, venue.Id);
            _seatRepository.BulkInsertSeats(moderate, SeatType.Moderate, venue.Id);
            _seatRepository.BulkInsertSeats(premier, SeatType.Premier, venue.Id);
        }

        public IVenueViewModel ReturnVenueViewModel(IVenueViewModel viewModel)
        {
            Venue venue = CheckVenueNullValue(viewModel.Venue.Id);
            venue.Address = _addressRepository.Find(venue.AddressId);

            Mapper.Map(venue, viewModel.Venue);
            viewModel.SeatCapacity = _seatService.GetSeatCapacities(venue.Id);

            return viewModel;
        }

        public void EditVenue(IVenueViewModel venueViewModel, int id)
        {
            Venue venueToEdit = CheckVenueNullValue(id);

            venueToEdit.AddressId = venueViewModel.Venue.AddressId;
            venueToEdit.Name = venueViewModel.Venue.Name;
            venueToEdit.ImageUrl = venueViewModel.Venue.ImageUrl;
            venueToEdit.Address.StreetAddress = venueViewModel.Venue.AddressDto.StreetAddress;
            venueToEdit.Address.City = venueViewModel.Venue.AddressDto.City;
            venueToEdit.Address.State = venueViewModel.Venue.AddressDto.State;
            venueToEdit.Address.ZipCode = venueViewModel.Venue.AddressDto.ZipCode;
            venueToEdit.IsActive = true;
            
            var capacity = _seatService.GetSeatCapacities(id);

            var seatsRemoved = new SeatCapacity
            {
                Budget = venueViewModel.SeatCapacity.Budget,
                Moderate = venueViewModel.SeatCapacity.Moderate,
                Premier = venueViewModel.SeatCapacity.Premier
            };

            var newSeatCapacity = new SeatCapacity
            {
                Budget = seatsRemoved.Budget - capacity.Budget,
                Moderate = seatsRemoved.Moderate - capacity.Moderate,
                Premier = seatsRemoved.Premier - capacity.Premier
            };

            _seatService.ChangeAmountOfSeatsInContext(newSeatCapacity, venueToEdit.Id);

            _repository.Commit();
            _addressRepository.Commit();
        }

        public void DeleteVenue(int id)
        {
            var venue = CheckVenueNullValue(id);

            List<Performance> performances = _performanceRepository.Collection()
                .Where(p => p.VenueId == id).ToList();

//            venue.IsActive = false;
//            venue.Address.IsActive = false;
           
            foreach (var performance in performances)
            {
                _performanceRepository.Delete(performance.Id);
                _performanceRepository.Commit();
            }

            _repository.Delete(id);
            _addressRepository.Delete(venue.AddressId);
            _seatRepository.DeleteAllVenueSeats(id);
            _repository.Commit();
            _addressRepository.Commit();
        }
    }
}
